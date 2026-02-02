using Application.Common.Interfaces.Repositories;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UsersRepository(ApplicationDbContext context) : IUsersRepository
    {
        public async Task<User> AddAsync(User entity, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<User> DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            context.Users.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            context.Users.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}