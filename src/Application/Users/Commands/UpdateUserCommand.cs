using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands
{
    public record UpdateUserCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
    }

    public class UpdateUserCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<UpdateUserCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var id = new UserId(request.UserId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                u => UpdateEntity(u, request, cancellationToken),
                () => new UserNotFoundException(id));
        }

        private async Task<Either<UserException, User>> UpdateEntity(
            User entity,
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.FullName, request.Email);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledUserException(entity.Id, ex);
            }
        }
    }
}