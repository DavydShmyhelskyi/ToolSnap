using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Models.Roles;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands
{
    public record CreateRoleCommand : IRequest<Either<RoleException, Role>>
    {
        public required string Name { get; init; }
    }

    public class CreateRoleCommandHandler(
        IRolesQueries queries,
        IRolesRepository repository)
        : IRequestHandler<CreateRoleCommand, Either<RoleException, Role>>
    {
        public async Task<Either<RoleException, Role>> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await queries.GetByTitleAsync(request.Name, cancellationToken);

            return await existing.MatchAsync(
                r => new RoleAlreadyExistsException(r.Id),
                () => CreateEntity(request, cancellationToken));
        }

        private async Task<Either<RoleException, Role>> CreateEntity(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var newRole = Role.New(request.Name);
                var result = await repository.AddAsync(newRole, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return new UnhandledRoleException(RoleId.Empty(), ex);
            }
        }
    }
}