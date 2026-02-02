using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolPhotos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolPhotosRepository(ApplicationDbContext context) : IToolPhotosRepository
    {
        public async Task<ToolPhoto> AddAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            await context.Set<ToolPhoto>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolPhoto> DeleteAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            context.Set<ToolPhoto>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolPhoto> UpdateAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            context.Set<ToolPhoto>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}