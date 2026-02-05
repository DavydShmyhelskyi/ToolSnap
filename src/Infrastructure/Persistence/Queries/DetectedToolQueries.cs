using Application.Common.Interfaces.Queries;
using Domain.Models.DetectedTools;
using Domain.Models.PhotoSessions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class DetectedToolQueries(ApplicationDbContext context) : IDetectedToolQueries
    {
        public async Task<IReadOnlyList<DetectedTool>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.DetectedTools.ToListAsync(cancellationToken);
        }

        public async Task<Option<DetectedTool>> GetByIdAsync(DetectedToolId detectedToolId, CancellationToken cancellationToken)
        {
            var detectedTool = await context.DetectedTools.FirstOrDefaultAsync(dt => dt.Id == detectedToolId, cancellationToken);
            return detectedTool != null ? Option<DetectedTool>.Some(detectedTool) : Option<DetectedTool>.None;
        }

        public async Task<IReadOnlyList<DetectedTool>> GetByPhotoSessionIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken)
        {
            return await context.DetectedTools.Where(dt => dt.PhotoSessionId == photoSessionId).ToListAsync(cancellationToken);
        }
    }
}