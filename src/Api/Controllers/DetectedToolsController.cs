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