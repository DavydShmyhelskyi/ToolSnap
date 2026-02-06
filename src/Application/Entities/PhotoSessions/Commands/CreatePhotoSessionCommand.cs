using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.PhotoSessions.Exceptions;
using Domain.Models.PhotoSessions;
using LanguageExt;
using MediatR;

namespace Application.Entities.PhotoSessions.Commands
{
    public record CreatePhotoSessionCommand : IRequest<Either<PhotoSessionException, PhotoSession>>
    {
        public required double Latitude { get; init; }
        public required double Longitude { get; init; }
        public required Guid ActionTypeId { get; init; }
    }

    public class CreatePhotoSessionCommandHandler(
        IPhotoSessionsRepository repository,
        IActionTypeQueries actionTypeQueries)
        : IRequestHandler<CreatePhotoSessionCommand, Either<PhotoSessionException, PhotoSession>>
    {
        public async Task<Either<PhotoSessionException, PhotoSession>> Handle(
            CreatePhotoSessionCommand request,
            CancellationToken cancellationToken)
        {
            var actionTypeId = new ActionTypeId(request.ActionTypeId);

            // Перевірка існування ActionType
            var actionType = await actionTypeQueries.GetByIdAsync(actionTypeId, cancellationToken);
            if (actionType.IsNone)
                return new ActionTypeNotFoundForPhotoSessionException(actionTypeId);

            return await CreateEntity(request, cancellationToken);
        }

        private async Task<Either<PhotoSessionException, PhotoSession>> CreateEntity(
            CreatePhotoSessionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var actionTypeId = new ActionTypeId(request.ActionTypeId);

                var newPhotoSession = PhotoSession.New(
                    request.Latitude,
                    request.Longitude,
                    actionTypeId);

                var result = await repository.AddAsync(newPhotoSession, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledPhotoSessionException(PhotoSessionId.Empty(), ex);
            }
        }
    }
}