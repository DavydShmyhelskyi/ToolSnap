using Domain.Models.DetectedTools;
using Domain.Models.PhotoSessions;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IDetectedToolQueries
    {
        Task<IReadOnlyList<DetectedTool>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<DetectedTool>> GetByIdAsync(DetectedToolId detectedToolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DetectedTool>> GetByPhotoSessionIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken);
    }
}