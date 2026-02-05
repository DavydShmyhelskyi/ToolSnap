using Domain.Models.Roles;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<Role> AddAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken);
        Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken);
    }
}
