using Application.Common.Interfaces.Repositories;
using Domain.Models.Users;

namespace Infrastructure.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public Task<User> AddAsync(User entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
