using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands
{
    public record DeleteUserCommand : IRequest<Either<UserException, User>>
    {
        public required Guid UserId { get; init; }
    }

    public class DeleteUserCommandHandler(
        IUsersQueries queries,
        IUsersRepository repository)
        : IRequestHandler<DeleteUserCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            var id = new UserId(request.UserId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<UserException, User>>(
                u => repository.DeleteAsync(u, cancellationToken).Result,
                () => new UserNotFoundException(id));
        }
    }
}