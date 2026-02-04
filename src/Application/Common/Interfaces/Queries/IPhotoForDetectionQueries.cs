using Domain.Models.PhotoSessions;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IPhotoForDetectionQueries
    {
        Task<IReadOnlyList<PhotoForDetection>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<PhotoForDetection>> GetByIdAsync(PhotoForDetectionId photoForDetectionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<PhotoForDetection>> GetByPhotoSessionIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken);
    }
}