using Application.Common.Interfaces.Repositories;
using Domain.Models.DetectedTools;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class DetectedToolRepository(ApplicationDbContext context) : IDetectedToolRepository
    {
        public async Task<DetectedTool> AddAsync(DetectedTool entity, CancellationToken cancellationToken)
        {
            await context.DetectedTools.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<DetectedTool> DeleteAsync(DetectedTool entity, CancellationToken cancellationToken)
        {
            context.DetectedTools.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}