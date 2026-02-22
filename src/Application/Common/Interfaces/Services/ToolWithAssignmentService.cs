using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Services;
using Application.Entities.DetectedTools.Commands;
using Application.Entities.PhotoForDetections.Commands;
using Application.Entities.PhotoSessions.Commands;
using Application.Entities.ToolAssignments.Commands;
using Application.Entities.ToolPhotos.Commands;
using Application.Entities.Tools.Commands;
using Application.Entities.ToolAssignments.Exceptions;
using Domain.Models.Locations;
using Domain.Models.Tools;
using LanguageExt;
using MediatR;

namespace Application.Services
{
    public class ToolWithAssignmentService : IToolWithAssignmentService
    {
        private readonly IMediator _mediator;
        private readonly ILocationsQueries _locationsQueries;

        public ToolWithAssignmentService(
            IMediator mediator,
            ILocationsQueries locationsQueries)
        {
            _mediator = mediator;
            _locationsQueries = locationsQueries;
        }

        public async Task<Either<Exception, Tool>> CreateToolWithAssignmentAsync(
            Guid userId,
            Guid actionTypeId,
            Guid photoTypeId,
            Guid locationId,
            Guid toolTypeId,
            Guid toolStatusId,
            Guid? brandId,
            Guid? modelId,
            string? serialNumber,
            string originalName,
            Stream fileStream,
            CancellationToken cancellationToken)
        {
            try
            {
                // 0. Дістаємо Location, бо з нього беремо координати
                var locId = new LocationId(locationId);

                var locationOpt = await _locationsQueries.GetByIdAsync(locId, cancellationToken);
                if (locationOpt.IsNone)
                {
                    return new LocationNotFoundForToolAssignmentException(locId);
                }

                var location = locationOpt.Match(l => l, () => throw new InvalidOperationException());

                // 1. Буферизуємо файл, щоб використати одне й те саме фото для ToolPhoto та PhotoForDetection
                await using var imageBuffer = new MemoryStream();
                await fileStream.CopyToAsync(imageBuffer, cancellationToken);
                imageBuffer.Position = 0;

                Stream CreateImageStream()
                {
                    var ms = new MemoryStream(imageBuffer.ToArray());
                    ms.Position = 0;
                    return ms;
                }

                // 2. Створюємо Tool
                var createToolResult = await _mediator.Send(new CreateToolCommand
                {
                    ToolTypeId = toolTypeId,
                    BrandId = brandId,
                    ModelId = modelId,
                    ToolStatusId = toolStatusId,
                    SerialNumber = serialNumber
                }, cancellationToken);

                if (createToolResult.IsLeft)
                    return createToolResult.LeftAsEnumerable().First();

                var tool = createToolResult.RightAsEnumerable().First();

                // 3. Створюємо ToolPhoto для цього Tool (те саме фото)
                var createToolPhotoResult = await _mediator.Send(new CreateToolPhotoCommand
                {
                    ToolId = tool.Id.Value,
                    PhotoTypeId = photoTypeId,
                    OriginalName = originalName,
                    FileStream = CreateImageStream()
                }, cancellationToken);

                if (createToolPhotoResult.IsLeft)
                    return createToolPhotoResult.LeftAsEnumerable().First();

                // 4. Створюємо PhotoSession (координати — з Location)
                var createPhotoSessionResult = await _mediator.Send(new CreatePhotoSessionCommand
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    ActionTypeId = actionTypeId
                }, cancellationToken);

                if (createPhotoSessionResult.IsLeft)
                    return createPhotoSessionResult.LeftAsEnumerable().First();

                var photoSession = createPhotoSessionResult.RightAsEnumerable().First();

                // 5. Створюємо PhotoForDetection (те саме фото)
                var createPhotoForDetectionResult = await _mediator.Send(new CreatePhotoForDetectionCommand
                {
                    PhotoSessionId = photoSession.Id.Value,
                    OriginalName = originalName,
                    FileStream = CreateImageStream()
                }, cancellationToken);

                if (createPhotoForDetectionResult.IsLeft)
                    return createPhotoForDetectionResult.LeftAsEnumerable().First();

                var photoForDetection = createPhotoForDetectionResult.RightAsEnumerable().First();
                _ = photoForDetection; // тут нам не потрібно повертати це назовні, важливий side-effect

                // 6. Створюємо DetectedTool
                var createDetectedToolResult = await _mediator.Send(new CreateDetectedToolCommand
                {
                    PhotoSessionId = photoSession.Id.Value,
                    ToolTypeId = tool.ToolTypeId.Value,
                    BrandId = tool.BrandId?.Value,
                    ModelId = tool.ModelId?.Value,
                    SerialNumber = tool.SerialNumber,
                    Confidence = 1.0f,
                    RedFlagged = false
                }, cancellationToken);

                if (createDetectedToolResult.IsLeft)
                    return createDetectedToolResult.LeftAsEnumerable().First();

                var detectedTool = createDetectedToolResult.RightAsEnumerable().First();

                // 7. Створюємо ToolAssignment (Take)
                var createAssignmentResult = await _mediator.Send(new CreateToolAssignmentCommand
                {
                    TakenDetectedToolId = detectedTool.Id.Value,
                    ToolId = tool.Id.Value,
                    UserId = userId,
                    LocationId = locationId
                }, cancellationToken);

                if (createAssignmentResult.IsLeft)
                    return createAssignmentResult.LeftAsEnumerable().First();

                var assignment = createAssignmentResult.RightAsEnumerable().First();

                // 8. Чекаємо мінімум 1 секунду між Take і Return
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                // 9. Робимо Return:
                var returnAssignmentResult = await _mediator.Send(new ReturnToolAssignmentCommand
                {
                    ToolAssignmentId = assignment.Id.Value,
                    LocationId = locationId,
                    ReturnedDetectedToolId = detectedTool.Id.Value
                }, cancellationToken);

                if (returnAssignmentResult.IsLeft)
                    return returnAssignmentResult.LeftAsEnumerable().First();

                // 10. Нас назовні ці сутності не цікавлять — важливі лише side-effects.
                //     Повертаємо тільки сам Tool.
                return tool;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}