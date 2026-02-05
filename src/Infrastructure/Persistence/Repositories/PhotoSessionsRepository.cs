using Application.Common.Interfaces.Repositories;
using Domain.Models.PhotoSessions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PhotoSessionsRepository(ApplicationDbContext context) : IPhotoSessionsRepository
    {
        public async Task<PhotoSession> AddAsync(PhotoSession entity, CancellationToken cancellationToken)
        {
            await context.PhotoSessions.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<PhotoSession> DeleteAsync(PhotoSession entity, CancellationToken cancellationToken)
        {
            context.PhotoSessions.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}