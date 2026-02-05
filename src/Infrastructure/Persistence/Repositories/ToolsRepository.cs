using Application.Common.Interfaces.Repositories;
using Domain.Models.Tools;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolsRepository(ApplicationDbContext context) : IToolsRepository
    {
        public async Task<Tool> AddAsync(Tool entity, CancellationToken cancellationToken)
        {
            await context.Tools.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Tool> UpdateAsync(Tool entity, CancellationToken cancellationToken)
        {
            context.Tools.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Tool> DeleteAsync(Tool entity, CancellationToken cancellationToken)
        {
            context.Tools.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}