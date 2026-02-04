using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record ConfirmEmailCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
    }

    public class ConfirmEmailCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<ConfirmEmailCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            var id = new UserId(request.UserId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                u => ConfirmEmail(u, cancellationToken),
                () => new UserNotFoundException(id));
        }

        private async Task<Either<UserException, User>> ConfirmEmail(
            User entity,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.ConfirmEmail();
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledUserException(entity.Id, ex);
            }
        }
    }
}