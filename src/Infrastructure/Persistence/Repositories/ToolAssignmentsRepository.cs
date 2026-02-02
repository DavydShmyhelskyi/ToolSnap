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

        public async Task<ToolAssignment> DeleteAsync(ToolAssignment entity, CancellationToken cancellationToken)
        {
            context.ToolAssignments.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}