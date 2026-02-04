using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Users.Exceptions;
using Domain.Models.Roles;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public record CreateUserCommand : IRequest<Either<UserException, User>>
    {
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required Guid RoleId { get; init; }
        public required string Password { get; init; }
        public required bool IsActive { get; init; } = true;
    }

    public class CreateUserCommandHandler(
        IUsersRepository repository,
        IUsersQueries queries,
        IRolesQueries rolesQueries)
        : IRequestHandler<CreateUserCommand, Either<UserException, User>>
    {
        public async Task<Either<UserException, User>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var roleId = new RoleId(request.RoleId);

            // Перевірка існування Role
            var role = await rolesQueries.GetByIdAsync(roleId, cancellationToken);
            if (role.IsNone)
                return new RoleNotFoundForUserException(roleId);

            // Перевірка на існування User з таким Email
            var existing = await queries.GetByNameAsync(request.FullName, cancellationToken);

            return await existing.MatchAsync(
                u => new UserAlreadyExistsException(u.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<UserException, User>> CreateEntity(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var roleId = new RoleId(request.RoleId);
                var newUser = User.New(
                    request.FullName,
                    request.Email,
                    roleId,
                    request.Password,
                    request.IsActive);

                var result = await repository.AddAsync(newUser, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledUserException(UserId.Empty(), ex);
            }
        }
    }
}