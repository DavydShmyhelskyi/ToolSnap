using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoForDetections.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("photos-for-detection")]
    public class PhotoForDetectionsController(
        IPhotoForDetectionQueries queries,
        IPhotoForDetectionControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PhotoForDetectionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PhotoForDetectionDto>>> GetPhotosForDetection(
            CancellationToken cancellationToken)
        {
            var photos = await queries.GetAllAsync(cancellationToken);

            var result = photos
                .Select(PhotoForDetectionDto.FromDomain)
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PhotoForDetectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhotoForDetectionDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<PhotoForDetectionDto>>(
                photoDto => Ok(photoDto),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(PhotoForDetectionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoForDetectionDto>> Create(
            [FromBody] CreatePhotoForDetectionDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreatePhotoForDetectionCommand
            {
                PhotoSessionId = request.PhotoSessionId,
                OriginalName = request.OriginalName
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<PhotoForDetectionDto>>(
                photo => CreatedAtAction(
                    nameof(GetById),
                    new { id = photo.Id.Value },
                    PhotoForDetectionDto.FromDomain(photo)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePhotoForDetectionCommand
            {
                PhotoForDetectionId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}