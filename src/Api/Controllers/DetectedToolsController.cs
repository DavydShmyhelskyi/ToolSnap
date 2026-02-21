using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.DetectedTools.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("detected-tools")]
    public class DetectedToolsController(
        IDetectedToolQueries queries,
        IDetectedToolControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<DetectedToolDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<DetectedToolDto>>> GetDetectedTools(
            CancellationToken cancellationToken)
        {
            var detectedTools = await queries.GetAllAsync(cancellationToken);

            var result = detectedTools
                .Select(DetectedToolDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DetectedToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetectedToolDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<DetectedToolDto>>(
                detectedToolDto => Ok(detectedToolDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(DetectedToolDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetectedToolDto>> Create(
            [FromBody] CreateDetectedToolDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateDetectedToolCommand
            {
                PhotoSessionId = request.PhotoSessionId,
                ToolTypeId = request.ToolTypeId,
                BrandId = request.BrandId,
                ModelId = request.ModelId,
                SerialNumber = request.SerialNumber,
                Confidence = request.Confidence,
                RedFlagged = request.RedFlagged
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<DetectedToolDto>>(
                detectedTool => CreatedAtAction(
                    nameof(GetById),
                    new { id = detectedTool.Id.Value },
                    DetectedToolDto.FromDomain(detectedTool)),
                error => error.ToObjectResult());
        }

        [HttpPost("batch")]
        [ProducesResponseType(typeof(IReadOnlyList<DetectedToolDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<DetectedToolDto>>> CreateBatch(
            [FromBody] CreateDetectedToolsBatchDto request,
            CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
            {
                return BadRequest("Items collection must not be empty.");
            }

            var command = new CreateDetectedToolsCommand
            {
                Items = request.Items
                    .Select(item => new CreateDetectedToolsCommandItem
                    {
                        PhotoSessionId = item.PhotoSessionId,
                        ToolTypeId = item.ToolTypeId,
                        BrandId = item.BrandId,
                        ModelId = item.ModelId,
                        SerialNumber = item.SerialNumber,
                        Confidence = item.Confidence,
                        RedFlagged = item.RedFlagged
                    })
                    .ToList()
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<IReadOnlyList<DetectedToolDto>>>(
                detectedTools =>
                {
                    var dtos = detectedTools
                        .Select(DetectedToolDto.FromDomain)
                        .ToList();

                    // 201 без конкретного Location (бо це колекція)
                    return StatusCode(StatusCodes.Status201Created, (IReadOnlyList<DetectedToolDto>)dtos);
                },
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteDetectedToolCommand
            {
                DetectedToolId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}