using Application.Common.Interfaces.Repositories;
using Domain.Models.Tools;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolStatusRepository(ApplicationDbContext context) : IToolStatusRepository
    {
        public async Task<ToolStatus> AddAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            await context.Set<ToolStatus>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolStatus> DeleteAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            context.Set<ToolStatus>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolStatus> UpdateAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            context.Set<ToolStatus>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}