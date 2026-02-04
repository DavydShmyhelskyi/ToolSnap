using Domain.Models.PhotoSessions;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPhotoSessionsRepository
    {
        Task<PhotoSession> AddAsync(PhotoSession entity, CancellationToken cancellationToken);
        Task<PhotoSession> DeleteAsync(PhotoSession entity, CancellationToken cancellationToken);
    }
}