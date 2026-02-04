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
        IPhotoForDetectionRepository repository)
        : IRequestHandler<DeletePhotoForDetectionCommand, Either<PhotoForDetectionException, PhotoForDetection>>
    {
        public async Task<Either<PhotoForDetectionException, PhotoForDetection>> Handle(
            DeletePhotoForDetectionCommand request,
            CancellationToken cancellationToken)
        {
            var id = new PhotoForDetectionId(request.PhotoForDetectionId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<PhotoForDetectionException, PhotoForDetection>>(
                pfd => repository.DeleteAsync(pfd, cancellationToken).Result,
                () => new PhotoForDetectionNotFoundException(id));
        }
    }
}