using Domain.Models.DetectedTools;

namespace Application.Common.Interfaces.Repositories
{
    public interface IDetectedToolRepository
    {
        Task<DetectedTool> AddAsync(DetectedTool entity, CancellationToken cancellationToken);
        Task<DetectedTool> DeleteAsync(DetectedTool entity, CancellationToken cancellationToken);
    }
}