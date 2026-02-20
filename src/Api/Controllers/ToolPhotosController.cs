using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolPhotos.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-photos")]
    public class ToolPhotosController(
        IToolPhotosQueries queries,
        IToolPhotoControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ToolPhotoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolPhotoDto>>> GetToolPhotos(
            CancellationToken cancellationToken)
        {
            var toolPhotos = await queries.GetAllAsync(cancellationToken);

            var result = toolPhotos
                .Select(ToolPhotoDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToolPhotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolPhotoDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ToolPhotoDto>>(
                toolPhotoDto => Ok(toolPhotoDto),
                () => NotFound());
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ToolPhotoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolPhotoDto>> Create(
            [FromForm] CreateToolPhotoDto request,
            CancellationToken cancellationToken)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest("File is required.");

            var command = new CreateToolPhotoCommand
            {
                ToolId = request.ToolId,
                PhotoTypeId = request.PhotoTypeId,
                OriginalName = request.File.FileName,
                FileStream = request.File.OpenReadStream()
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolPhotoDto>>(
                toolPhoto => CreatedAtAction(
                    nameof(GetById),
                    new { id = toolPhoto.Id.Value },
                    ToolPhotoDto.FromDomain(toolPhoto)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteToolPhotoCommand
            {
                ToolPhotoId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }

        // GET /tool-photos/file?toolId={guid}&photoTypeTitle={title}
        [HttpGet("file")]
        [ProducesResponseType(typeof(ToolPhotoFileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFileByToolAndType(
            [FromQuery] Guid toolId,
            [FromQuery] string photoTypeTitle,
            CancellationToken cancellationToken)
        {
            var command = new GetToolPhotoFileByToolAndTypeCommand
            {
                ToolId = toolId,
                PhotoTypeTitle = photoTypeTitle
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                tuple =>
                {
                    var (fileName, content) = tuple;

                    if (string.IsNullOrEmpty(fileName) || content.Length == 0)
                        return NotFound();

                    var dto = new ToolPhotoFileDto(fileName, content);
                    return Ok(dto);
                },
                error => error.ToObjectResult());
        }

        // GET /tool-photos/tool/{toolId}/files
        [HttpGet("tool/{toolId:guid}/files")]
        [ProducesResponseType(typeof(IReadOnlyList<ToolPhotoFileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFilesByTool(
            Guid toolId,
            CancellationToken cancellationToken)
        {
            var command = new GetToolPhotoFilesByToolCommand
            {
                ToolId = toolId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                files =>
                {
                    var dtos = files
                        .Select(f => new ToolPhotoFileDto(f.FileName, f.Content))
                        .ToList();

                    return Ok(dtos);
                },
                error => error.ToObjectResult());
        }
    }
}