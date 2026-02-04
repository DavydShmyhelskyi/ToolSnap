using Domain.Models.PhotoSessions;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPhotoForDetectionRepository
    {
        Task<PhotoForDetection> AddAsync(PhotoForDetection entity, CancellationToken cancellationToken);
        Task<PhotoForDetection> DeleteAsync(PhotoForDetection entity, CancellationToken cancellationToken);
    }
}