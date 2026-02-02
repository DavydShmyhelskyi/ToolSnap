using Application.Common.Interfaces.Repositories;
using Domain.Models.Roles;

namespace Infrastructure.Persistence.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        public Task<Role> AddAsync(Role entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
