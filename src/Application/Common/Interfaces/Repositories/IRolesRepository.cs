using Domain.Models.Roles;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<Role> AddAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken);

        Task<Option<Role>> GetByIdAsync(RoleId id, CancellationToken cancellationToken);
        Task<Option<Role>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken);
    }
}
