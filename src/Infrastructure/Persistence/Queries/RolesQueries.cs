using Application.Common.Interfaces.Queries;
using Domain.Models.Locations;
using Domain.Models.Roles;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class RolesQueries(ApplicationDbContext context) : IRolesQueries
    {
        public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Option<Role>> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            return role != null ? Option<Role>.Some(role) : Option<Role>.None;
        }

        public async Task<Option<Role>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Title == name, cancellationToken);
            return role != null ? Option<Role>.Some(role) : Option<Role>.None;
        }
    }
}
