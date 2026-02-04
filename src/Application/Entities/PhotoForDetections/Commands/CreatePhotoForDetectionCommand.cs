using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoForDetections.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoForDetections.Commands
{
    public record CreatePhotoForDetectionCommand : IRequest<Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public required Guid PhotoSessionId { get; init; }
        public required string OriginalName { get; init; }
    }

    public class CreatePhotoForDetectionCommandHandler(
        IPhotoForDetectionRepository repository,
        IPhotoSessionsQueries photoSessionQueries)
        : IRequestHandler<CreatePhotoForDetectionCommand, Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public async Task<Either<PhotoForDetectionException, PhotoForDetection>> Handle(
            CreatePhotoForDetectionCommand request,
            CancellationToken cancellationToken)
        {
            var photoSessionId = new PhotoSessionId(request.PhotoSessionId);

            // Перевірка існування PhotoSession
            var photoSession = await photoSessionQueries.GetByIdAsync(photoSessionId, cancellationToken);
            if (photoSession.IsNone)
                return new PhotoSessionNotFoundForPhotoException(photoSessionId);

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<PhotoForDetectionException, PhotoForDetection>> CreateEntity(
            CreatePhotoForDetectionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var photoSessionId = new PhotoSessionId(request.PhotoSessionId);
                var photoForDetectionId = PhotoForDetectionId.New();

                var newPhoto = PhotoForDetection.New(
                    photoForDetectionId,
                    photoSessionId,
                    request.OriginalName);

                var result = await repository.AddAsync(newPhoto, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledPhotoForDetectionException(PhotoForDetectionId.Empty(), ex);
            }
        }
    }
}