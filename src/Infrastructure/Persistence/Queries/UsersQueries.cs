using Application.Common.Interfaces.Queries;
using Domain.Models.Roles;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class UsersQueries(ApplicationDbContext context) : IUsersQueries
    {
        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Users.ToListAsync(cancellationToken);
        }

        public async Task<Option<User>> GetByIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            return user != null ? Option<User>.Some(user) : Option<User>.None;
        }

        public async Task<IReadOnlyList<User>> GetAllByRoleAsync(RoleId roleId, CancellationToken cancellationToken)
        {
            return await context.Users.Where(u => u.RoleId == roleId).ToListAsync(cancellationToken);
        }

        public async Task<Option<User>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.FullName == name, cancellationToken);
            return user != null ? Option<User>.Some(user) : Option<User>.None;
        }
    }
}
