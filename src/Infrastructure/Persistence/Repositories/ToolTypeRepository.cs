using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolTypeRepository(ApplicationDbContext context) : IToolTypeRepository
    {
        public async Task<ToolType> AddAsync(ToolType entity, CancellationToken cancellationToken)
        {
            await context.ToolTypes.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolType> UpdateAsync(ToolType entity, CancellationToken cancellationToken)
        {
            context.ToolTypes.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolType> DeleteAsync(ToolType entity, CancellationToken cancellationToken)
        {
            context.ToolTypes.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}