using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Roles.Exceptions;
using Domain.Models.Roles;
using LanguageExt;
using MediatR;

namespace Application.Entities.Roles.Commands
{
    public record DeleteRoleCommand : IRequest<Either<RoleException, Role>>
    {
        public required Guid RoleId { get; init; }
    }

    public class DeleteRoleCommandHandler(
        IRolesQueries queries,
        IRolesRepository repository)
        : IRequestHandler<DeleteRoleCommand, Either<RoleException, Role>>
    {
        public async Task<Either<RoleException, Role>> Handle(
            DeleteRoleCommand request,
            CancellationToken cancellationToken)
        {
            var id = new RoleId(request.RoleId);
            var entity = await queries.GetByIdAsync(id, cancellationToken);

            return entity.Match<Either<RoleException, Role>>(
                r => repository.DeleteAsync(r, cancellationToken).Result,
                () => new RoleNotFoundException(id));
        }
    }
}