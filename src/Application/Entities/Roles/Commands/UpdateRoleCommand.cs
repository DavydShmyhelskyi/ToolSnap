using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Roles.Exceptions;
using Domain.Models.Roles;
using LanguageExt;
using MediatR;

namespace Application.Entities.Roles.Commands
{
    public record UpdateRoleCommand : IRequest<Either<RoleException, Role>>
    {
        public required Guid RoleId { get; init; }
        public required string Name { get; init; }
    }

    public class UpdateRoleCommandHandler(
        IRolesQueries queries,
        IRolesRepository repository)
        : IRequestHandler<UpdateRoleCommand, Either<RoleException, Role>>
    {
        public async Task<Either<RoleException, Role>> Handle(
            UpdateRoleCommand request,
            CancellationToken cancellationToken)
        {
            var id = new RoleId(request.RoleId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return await entity.MatchAsync(
                r => UpdateEntity(r, request, cancellationToken),
                () => new RoleNotFoundException(id));
        }

        private async Task<Either<RoleException, Role>> UpdateEntity(
            Role entity,
            UpdateRoleCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                entity.Update(request.Name);
                return await repository.UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledRoleException(entity.Id, ex);
            }
        }
    }
}