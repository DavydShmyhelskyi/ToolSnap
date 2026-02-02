using Application.Common.Interfaces.Queries;
using Domain.Models.Roles;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class RolesQueries(ApplicationDbContext context) : IRolesQueries
    {
        public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Roles
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<Role>> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            return role == null ? Option<Role>.None : Option<Role>.Some(role);
        }

        public async Task<Option<Role>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
            return role == null ? Option<Role>.None : Option<Role>.Some(role);
        }
    }
}
