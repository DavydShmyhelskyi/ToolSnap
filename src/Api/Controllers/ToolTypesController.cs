using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ToolTypes.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("tool-types")]
    public class ToolTypesController(
        IToolTypeQueries queries,
        IToolTypeControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ToolTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ToolTypeDto>>> GetToolTypes(
            CancellationToken cancellationToken)
        {
            var toolTypes = await queries.GetAllAsync(cancellationToken);

            var result = toolTypes
                .Select(t => new ToolTypeDto(t.Id.Value, t.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ToolTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolTypeDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ToolTypeDto>>(
                toolType => Ok(new ToolTypeDto(toolType.Id, toolType.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToolTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToolTypeDto>> Create(
            [FromBody] CreateToolTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateToolTypeCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTypeDto>>(
                toolType => CreatedAtAction(
                    nameof(GetById),
                    new { id = toolType.Id.Value },
                    new ToolTypeDto(toolType.Id.Value, toolType.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ToolTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolTypeDto>> Update(
            Guid id,
            [FromBody] UpdateToolTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateToolTypeCommand
            {
                ToolTypeId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ToolTypeDto>>(
                toolType => Ok(new ToolTypeDto(toolType.Id.Value, toolType.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteToolTypeCommand
            {
                ToolTypeId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}