using Application.Common.Interfaces.Repositories;
using Domain.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RolesRepository(ApplicationDbContext context) : IRolesRepository
    {
        public async Task<Role> AddAsync(Role entity, CancellationToken cancellationToken)
        {
            await context.Roles.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Role> DeleteAsync(Role entity, CancellationToken cancellationToken)
        {
            context.Roles.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
        {
            context.Roles.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}