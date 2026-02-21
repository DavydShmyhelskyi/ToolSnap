using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoForDetections.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoForDetections.Commands
{
    public record CreatePhotoForDetectionCommand
    : IRequest<Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public required Guid PhotoSessionId { get; init; }
        public required string OriginalName { get; init; }
        public required Stream FileStream { get; init; }
    }

    public class CreatePhotoForDetectionCommandHandler(
    IPhotoForDetectionRepository repository,
    IPhotoSessionsQueries photoSessionQueries,
    IFileStorage fileStorage)
    : IRequestHandler<CreatePhotoForDetectionCommand, Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public async Task<Either<PhotoForDetectionException, PhotoForDetection>> Handle(
            CreatePhotoForDetectionCommand request,
            CancellationToken cancellationToken)
        {
            var photoSessionId = new PhotoSessionId(request.PhotoSessionId);

            var photoSession = await photoSessionQueries.GetByIdAsync(photoSessionId, cancellationToken);
            if (photoSession.IsNone)
                return new PhotoSessionNotFoundForPhotoException(photoSessionId);

            return await CreateAndUpload(request, photoSessionId, cancellationToken);
        }

        private async Task<Either<PhotoForDetectionException, PhotoForDetection>> CreateAndUpload(
            CreatePhotoForDetectionCommand request,
            PhotoSessionId photoSessionId,
            CancellationToken cancellationToken)
        {
            var entity = PhotoForDetection.New(
                photoSessionId,
                request.OriginalName);

            try
            {
                // 1?? Спочатку upload
                await fileStorage.UploadAsync(
                    request.FileStream,
                    entity.GetFilePath(),
                    cancellationToken);

                // 2?? Потім збереження в БД
                await repository.AddAsync(entity, cancellationToken);

                return entity;
            }
            catch (Exception ex)
            {
                return new UnhandledPhotoForDetectionException(entity.Id, ex);
            }
        }
    }
}