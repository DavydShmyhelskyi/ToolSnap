using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolPhotos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PhotoTypeRepository(ApplicationDbContext context) : IPhotoTypeRepository
    {
        public async Task<PhotoType> AddAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            await context.PhotoTypes.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<PhotoType> UpdateAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            context.PhotoTypes.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<PhotoType> DeleteAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            context.PhotoTypes.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}