using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoForDetections.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoForDetections.Commands
{
    public record DeletePhotoForDetectionCommand : IRequest<Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public required Guid PhotoForDetectionId { get; init; }
    }

    public class DeletePhotoForDetectionCommandHandler(
    IPhotoForDetectionQueries queries,
    IPhotoForDetectionRepository repository,
    IFileStorage fileStorage)
    : IRequestHandler<DeletePhotoForDetectionCommand, Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public async Task<Either<PhotoForDetectionException, PhotoForDetection>> Handle(
            DeletePhotoForDetectionCommand request,
            CancellationToken cancellationToken)
        {
            var id = new PhotoForDetectionId(request.PhotoForDetectionId);
            var entityOption = await queries.GetByIdAsync(id, cancellationToken);

            return await entityOption.MatchAsync(
                async entity =>
                {
                    try
                    {
                        // 1️⃣ Спочатку видаляємо файл
                        await fileStorage.DeleteAsync(
                            entity.GetFilePath(),
                            cancellationToken);

                        // 2️⃣ Потім запис у БД
                        await repository.DeleteAsync(entity, cancellationToken);

                        return entity;
                    }
                    catch (Exception ex)
                    {
                        return new UnhandledPhotoForDetectionException(id, ex);
                    }
                },
                () => Task.FromResult<Either<PhotoForDetectionException, PhotoForDetection>>(
                    new PhotoForDetectionNotFoundException(id))
            );
        }
    }
}