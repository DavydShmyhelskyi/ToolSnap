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
        [ProducesResponseType(typeof(ToolPhotoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolPhotoDto>> Create(
            [FromBody] CreateToolPhotoDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateToolPhotoCommand
            {
                ToolId = request.ToolId,
                PhotoTypeId = request.PhotoTypeId,
                OriginalName = request.OriginalName
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
    }
}