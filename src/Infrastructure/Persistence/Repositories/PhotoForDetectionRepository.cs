using Application.Common.Interfaces.Repositories;
using Domain.Models.PhotoSessions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PhotoForDetectionRepository(ApplicationDbContext context) : IPhotoForDetectionRepository
    {
        public async Task<PhotoForDetection> AddAsync(PhotoForDetection entity, CancellationToken cancellationToken)
        {
            await context.PhotosForDetection.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<PhotoForDetection> DeleteAsync(PhotoForDetection entity, CancellationToken cancellationToken)
        {
            context.PhotosForDetection.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}