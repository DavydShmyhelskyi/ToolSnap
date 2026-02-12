using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoTypes.Commands;
using Api.Modules.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("photo-types")]
    public class PhotoTypesController(
        IPhotoTypeQueries queries,
        IPhotoTypeControllerService service,
        ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PhotoTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PhotoTypeDto>>> GetPhotoTypes(
            CancellationToken cancellationToken)
        {
            var photoTypes = await queries.GetAllAsync(cancellationToken);

            var result = photoTypes
                .Select(p => new PhotoTypeDto(p.Id.Value, p.Title))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PhotoTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhotoTypeDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var entity = await service.Get(id, cancellationToken);

            return entity.Match<ActionResult<PhotoTypeDto>>(
                photoType => Ok(new PhotoTypeDto(photoType.Id, photoType.Title)),
                () => NotFound());
        }

        [HttpPost]
        [ProducesResponseType(typeof(PhotoTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoTypeDto>> Create(
            [FromBody] CreatePhotoTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreatePhotoTypeCommand
            {
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<PhotoTypeDto>>(
                photoType => CreatedAtAction(
                    nameof(GetById),
                    new { id = photoType.Id.Value },
                    new PhotoTypeDto(photoType.Id.Value, photoType.Title)),
                error => error.ToObjectResult());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PhotoTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PhotoTypeDto>> Update(
            Guid id,
            [FromBody] UpdatePhotoTypeDto request,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePhotoTypeCommand
            {
                PhotoTypeId = id,
                Title = request.Title
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<PhotoTypeDto>>(
                photoType => Ok(new PhotoTypeDto(photoType.Id.Value, photoType.Title)),
                error => error.ToObjectResult());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePhotoTypeCommand
            {
                PhotoTypeId = id
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                error => error.ToObjectResult());
        }
    }
}