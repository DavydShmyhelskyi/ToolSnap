using Domain.Models.PhotoSessions;
using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IPhotoSessionsQueries
    {
        Task<IReadOnlyList<PhotoSession>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<PhotoSession>> GetByIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<PhotoSession>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    }
}