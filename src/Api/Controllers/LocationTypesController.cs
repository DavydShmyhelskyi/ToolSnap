using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.LocationTypes.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("location-types")]
    public class LocationTypesController(
        ILocationTypeQueries queries,
        ILocationTypeControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<LocationTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<LocationTypeDto>>> GetLocationTypes(
            CancellationToken cancellationToken)
        {
            var locationTypes = await queries.GetAllAsync(cancellationToken);

            var result = locationTypes
                .Select(l => new LocationTypeDto(l.Id.Value, l.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(LocationTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationTypeDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<LocationTypeDto>>(
                locationType => Ok(new LocationTypeDto(locationType.Id, locationType.Title)),
                () => NotFound());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationTypeDto>> Create(
            [FromBody] CreateLocationTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateLocationTypeCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationTypeDto>>(
                locationType => CreatedAtAction(
                    nameof(GetById),
                    new { id = locationType.Id.Value },
                    new LocationTypeDto(locationType.Id.Value, locationType.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationTypeDto>> Update(
            Guid id,
            [FromBody] UpdateLocationTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLocationTypeCommand
            {
                LocationTypeId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationTypeDto>>(
                locationType => Ok(new LocationTypeDto(locationType.Id.Value, locationType.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteLocationTypeCommand
            {
                LocationTypeId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}