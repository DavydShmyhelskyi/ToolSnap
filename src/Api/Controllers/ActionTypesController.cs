using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.ActionTypes.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("action-types")]
    public class ActionTypesController(
        IActionTypeQueries queries,
        IActionTypeControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ActionTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ActionTypeDto>>> GetActionTypes(
            CancellationToken cancellationToken)
        {
            var actionTypes = await queries.GetAllAsync(cancellationToken);

            var result = actionTypes
                .Select(a => new ActionTypeDto(a.Id.Value, a.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ActionTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActionTypeDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ActionTypeDto>>(
                actionType => Ok(new ActionTypeDto(actionType.Id, actionType.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActionTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActionTypeDto>> Create(
            [FromBody] CreateActionTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateActionTypeCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ActionTypeDto>>(
                actionType => CreatedAtAction(
                    nameof(GetById),
                    new { id = actionType.Id.Value },
                    new ActionTypeDto(actionType.Id.Value, actionType.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ActionTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActionTypeDto>> Update(
            Guid id,
            [FromBody] UpdateActionTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateActionTypeCommand
            {
                ActionTypeId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ActionTypeDto>>(
                actionType => Ok(new ActionTypeDto(actionType.Id.Value, actionType.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteActionTypeCommand
            {
                ActionTypeId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}