using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record ActivateUserCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
    }

    public class ActivateUserCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<ActivateUserCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            ActivateUserCommand request,
            CancellationToken cancellationToken)
        {
            var id = new UserId(request.UserId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                u => ActivateUser(u, cancellationToken),
                () => new UserNotFoundException(id));
        }

        private async Task<Either<UserException, User>> ActivateUser(
            User entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Activate();
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledUserException(entity.Id, ex);
            }
        }
    }
}