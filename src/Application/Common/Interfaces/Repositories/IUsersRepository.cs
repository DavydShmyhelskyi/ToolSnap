using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<User> AddAsync(User entity, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User entity, CancellationToken cancellationToken);
        Task<User> DeleteAsync(User entity, CancellationToken cancellationToken);

    }
}
