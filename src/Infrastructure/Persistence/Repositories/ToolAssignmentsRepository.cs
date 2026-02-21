using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolAssignments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolAssignmentsRepository(ApplicationDbContext context) : IToolAssignmentsRepository
    {
        public async Task<ToolAssignment> AddAsync(ToolAssignment entity, CancellationToken cancellationToken)
        {
            await context.ToolAssignments.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<IReadOnlyList<ToolAssignment>> AddRangeAsync(
            IEnumerable<ToolAssignment> entities,
            CancellationToken cancellationToken)
        {
            var list = entities.ToList();
            if (list.Count == 0)
                return Array.Empty<ToolAssignment>();

            await context.ToolAssignments.AddRangeAsync(list, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return list;
        }

        public async Task<ToolAssignment> UpdateAsync(ToolAssignment entity, CancellationToken cancellationToken)
        {
            context.ToolAssignments.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<IReadOnlyList<ToolAssignment>> UpdateRangeAsync(
            IEnumerable<ToolAssignment> entities,
            CancellationToken cancellationToken)
        {
            var list = entities.ToList();
            if (list.Count == 0)
                return Array.Empty<ToolAssignment>();

            context.ToolAssignments.UpdateRange(list);
            await context.SaveChangesAsync(cancellationToken);
            return list;
        }

        public async Task<ToolAssignment> DeleteAsync(ToolAssignment entity, CancellationToken cancellationToken)
        {
            context.ToolAssignments.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}