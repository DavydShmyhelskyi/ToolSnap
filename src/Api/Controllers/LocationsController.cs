using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.Locations.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("locations")]
    public class LocationsController(
        ILocationsQueries queries,
        ILocationControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<LocationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<LocationDto>>> GetLocations(
            CancellationToken cancellationToken)
        {
            var locations = await queries.GetAllAsync(cancellationToken);

            var result = locations
                .Select(l => new LocationDto(
                    l.Id.Value,
                    l.Name,
                    l.LocationTypeId.Value,
                    l.Address,
                    l.Latitude,
                    l.Longitude,
                    l.IsActive,
                    l.CreatedAt))
                .ToList();

            return Ok(result);
        }
        [HttpGet("nearest")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> GetNearest(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            CancellationToken cancellationToken)
        {
            const double radiusMeters = 50;

            var locationOption = await queries.GetNearestAsync(
                latitude,
                longitude,
                radiusMeters,
                cancellationToken);

            return locationOption.Match<ActionResult<LocationDto>>(
                location => Ok(new LocationDto(
                    location.Id.Value,
                    location.Name,
                    location.LocationTypeId.Value,
                    location.Address,
                    location.Latitude,
                    location.Longitude,
                    location.IsActive,
                    location.CreatedAt)),
                () => NotFound());
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<LocationDto>>(
                locationDto => Ok(locationDto),
                () => NotFound());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationDto>> Create(
            [FromBody] CreateLocationDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateLocationCommand
            {
                Name = request.Name,
                LocationTypeId = request.LocationTypeId,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsActive = request.IsActive
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationDto>>(
                location => CreatedAtAction(
                    nameof(GetById),
                    new { id = location.Id.Value },
                    LocationDto.FromDomain(location)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> Update(
            Guid id,
            [FromBody] UpdateLocationDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLocationCommand
            {
                LocationId = id,
                Name = request.Name,
                LocationTypeId = request.LocationTypeId,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationDto>>(
                location => Ok(LocationDto.FromDomain(location)),
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
            var command = new DeleteLocationCommand
            {
                LocationId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/activate")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> Activate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ActivateLocationCommand
            {
                LocationId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationDto>>(
                location => Ok(LocationDto.FromDomain(location)),
                error => error.ToObjectResult());
        }

        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> Deactivate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeactivateLocationCommand
            {
                LocationId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<LocationDto>>(
                location => Ok(LocationDto.FromDomain(location)),
                error => error.ToObjectResult());
        }
    }
}