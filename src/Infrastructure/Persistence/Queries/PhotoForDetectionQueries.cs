using Application.Common.Interfaces.Queries;
using Domain.Models.PhotoSessions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class PhotoForDetectionQueries(ApplicationDbContext context) : IPhotoForDetectionQueries
    {
        public async Task<IReadOnlyList<PhotoForDetection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.PhotosForDetection.ToListAsync(cancellationToken);
        }

        public async Task<Option<PhotoForDetection>> GetByIdAsync(PhotoForDetectionId photoForDetectionId, CancellationToken cancellationToken)
        {
            var photo = await context.PhotosForDetection.FirstOrDefaultAsync(p => p.Id == photoForDetectionId, cancellationToken);
            return photo != null ? Option<PhotoForDetection>.Some(photo) : Option<PhotoForDetection>.None;
        }

        public async Task<IReadOnlyList<PhotoForDetection>> GetByPhotoSessionIdAsync(PhotoSessionId photoSessionId, CancellationToken cancellationToken)
        {
            return await context.PhotosForDetection.Where(p => p.PhotoSessionId == photoSessionId).ToListAsync(cancellationToken);
        }
    }
}