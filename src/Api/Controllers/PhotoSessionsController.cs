using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoSessions.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("photo-sessions")]
    public class PhotoSessionsController(
        IPhotoSessionsQueries queries,
        IPhotoSessionControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PhotoSessionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PhotoSessionDto>>> GetPhotoSessions(
            CancellationToken cancellationToken)
        {
            var photoSessions = await queries.GetAllAsync(cancellationToken);

            var result = photoSessions
                .Select(PhotoSessionDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PhotoSessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhotoSessionDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<PhotoSessionDto>>(
                photoSessionDto => Ok(photoSessionDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(PhotoSessionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoSessionDto>> Create(
            [FromBody] CreatePhotoSessionDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreatePhotoSessionCommand
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                ActionTypeId = request.ActionTypeId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<PhotoSessionDto>>(
                photoSession => CreatedAtAction(
                    nameof(GetById),
                    new { id = photoSession.Id.Value },
                    PhotoSessionDto.FromDomain(photoSession)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePhotoSessionCommand
            {
                PhotoSessionId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}