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
            => await context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<Option<User>> GetByIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            
            return user == null ? Option<User>.None : Option<User>.Some(user);
        }

        public async Task<IReadOnlyList<User>> GetAllByRoleAsync(RoleId roleId, CancellationToken cancellationToken)
            => await context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<Option<User>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.FullName.ToLower() == name.ToLower(), cancellationToken);
            
            return user == null ? Option<User>.None : Option<User>.Some(user);
        }

        public async Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
            
            return user == null ? Option<User>.None : Option<User>.Some(user);
        }
    }
}
