using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoSessions.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoSessions.Commands
{
    public record DeletePhotoSessionCommand : IRequest<Either<PhotoSessionException, PhotoSession>>
    {
        public required Guid PhotoSessionId { get; init; }
    }

    public class DeletePhotoSessionCommandHandler(
        IPhotoSessionsQueries queries,
        IPhotoSessionsRepository repository)
        : IRequestHandler<DeletePhotoSessionCommand, Either<PhotoSessionException, PhotoSession>>
    {
        public async Task<Either<PhotoSessionException, PhotoSession>> Handle(
            DeletePhotoSessionCommand request,
            CancellationToken cancellationToken)
        {
            var id = new PhotoSessionId(request.PhotoSessionId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<PhotoSessionException, PhotoSession>>(
                ps => repository.DeleteAsync(ps, cancellationToken).Result,
                () => new PhotoSessionNotFoundException(id));
        }
    }
}