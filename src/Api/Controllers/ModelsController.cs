using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Models.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("models")]
    public class ModelsController(
        IModelQueries queries,
        IModelControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ModelDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ModelDto>>> GetModels(
            CancellationToken cancellationToken)
        {
            var models = await queries.GetAllAsync(cancellationToken);

            var result = models
                .Select(m => new ModelDto(m.Id.Value, m.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModelDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<ModelDto>>(
                model => Ok(new ModelDto(model.Id, model.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModelDto>> Create(
            [FromBody] CreateModelDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateModelCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ModelDto>>(
                model => CreatedAtAction(
                    nameof(GetById),
                    new { id = model.Id.Value },
                    new ModelDto(model.Id.Value, model.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModelDto>> Update(
            Guid id,
            [FromBody] UpdateModelDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateModelCommand
            {
                ModelId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<ModelDto>>(
                model => Ok(new ModelDto(model.Id.Value, model.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteModelCommand
            {
                ModelId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}