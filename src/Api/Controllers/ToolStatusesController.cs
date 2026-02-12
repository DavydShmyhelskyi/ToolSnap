using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolStatuses.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-statuses")]
    public class ToolStatusesController(
        IToolStatusQueries queries,
        IToolStatusControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ToolStatusDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolStatusDto>>> GetToolStatuses(
            CancellationToken cancellationToken)
        {
            var toolStatuses = await queries.GetAllAsync(cancellationToken);

            var result = toolStatuses
                .Select(t => new ToolStatusDto(t.Id.Value, t.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToolStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolStatusDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ToolStatusDto>>(
                toolStatus => Ok(new ToolStatusDto(toolStatus.Id, toolStatus.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToolStatusDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolStatusDto>> Create(
            [FromBody] CreateToolStatusDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateToolStatusCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolStatusDto>>(
                toolStatus => CreatedAtAction(
                    nameof(GetById),
                    new { id = toolStatus.Id.Value },
                    new ToolStatusDto(toolStatus.Id.Value, toolStatus.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ToolStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolStatusDto>> Update(
            Guid id,
            [FromBody] UpdateToolStatusDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateToolStatusCommand
            {
                ToolStatusId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolStatusDto>>(
                toolStatus => Ok(new ToolStatusDto(toolStatus.Id.Value, toolStatus.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteToolStatusCommand
            {
                ToolStatusId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}