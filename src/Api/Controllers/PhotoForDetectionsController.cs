using Api.DTOs;
using Api.Modules.Errors;
using Api.Services.Abstract;
using Api.Services.GemeniAiService;
using Application.Common.Interfaces.Queries;
using Application.Entities.PhotoForDetections.Commands;
using Application.Entities.PhotoSessions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("photos-for-detection")]
    public class PhotoForDetectionsController(
        IPhotoForDetectionQueries queries,
        IPhotoForDetectionControllerService service,
        ISender sender,
        GeminiService geminiService) : ControllerBase
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

        [HttpGet("session/{sessionId:guid}/files")]
        [ProducesResponseType(typeof(IReadOnlyList<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFilesBySession(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            var command = new GetPhotoFilesBySessionCommand
            {
                PhotoSessionId = sessionId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                Right: files =>
                {
                    if (!files.Any())
                        return NotFound();

                    var response = files.Select(f => new
                    {
                        f.FileName,
                        ContentBase64 = Convert.ToBase64String(f.Content)
                    });

                    return Ok(response);
                },
                Left: ex => ex.ToObjectResult()
            );
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(PhotoForDetectionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoForDetectionDto>> Create(
            [FromForm] CreatePhotoForDetectionDto request,
            CancellationToken cancellationToken)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest("File is required.");

            await using var stream = request.File.OpenReadStream();

            var command = new CreatePhotoForDetectionCommand
            {
                PhotoSessionId = request.PhotoSessionId,
                OriginalName = request.File.FileName,
                FileStream = stream
            };

            var result = await sender.Send(command, cancellationToken);

            return result.Match<ActionResult<PhotoForDetectionDto>>(
                photo => CreatedAtAction(
                    nameof(GetById),
                    new { id = photo.Id.Value },
                    PhotoForDetectionDto.FromDomain(photo)),
                error => error.ToObjectResult());
        }

        [HttpPost("detect/{sessionId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DetectTools(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            if (sessionId == Guid.Empty)
                return BadRequest("SessionId is required.");

            try
            {
                var response = await geminiService.DetectToolsFromSessionAsync(
                    sessionId,
                    cancellationToken);

                return Ok(new { detection = response });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = $"An error occurred during tool detection: {ex.Message}" });
            }
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