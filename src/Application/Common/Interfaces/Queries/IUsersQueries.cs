using Domain.Models.Roles;
using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IUsersQueries
    {
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<User>> GetByIdAsync(UserId toolPhotoId, CancellationToken cancellationToken);
        Task<IReadOnlyList<User>> GetAllByRoleAsync(RoleId roleId, CancellationToken cancellationToken);
        Task<Option<User>> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
