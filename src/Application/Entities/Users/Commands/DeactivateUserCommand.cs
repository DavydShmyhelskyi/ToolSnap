using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record DeactivateUserCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
    }

    public class DeactivateUserCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<DeactivateUserCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            DeactivateUserCommand request,
            CancellationToken cancellationToken)
        {
            var id = new UserId(request.UserId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                u => DeactivateUser(u, cancellationToken),
                () => new UserNotFoundException(id));
        }

        private async Task<Either<UserException, User>> DeactivateUser(
            User entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Deactivate();
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledUserException(entity.Id, ex);
            }
        }
    }
}