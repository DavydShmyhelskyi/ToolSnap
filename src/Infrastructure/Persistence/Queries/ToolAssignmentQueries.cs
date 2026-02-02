using Application.Common.Interfaces.Queries;
using Domain.Models.ToolAssignments;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolAssignmentQueries(ApplicationDbContext context) : IToolAssignmentQueries
    {
        public async Task<IReadOnlyList<ToolAssignment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolAssignments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolAssignment>> GetByIdAsync(ToolAssignmentId toolId, CancellationToken cancellationToken)
        {
            var toolAssignment = await context.ToolAssignments
                .AsNoTracking()
                .FirstOrDefaultAsync(ta => ta.Id == toolId, cancellationToken);
            return toolAssignment == null ? Option<ToolAssignment>.None : Option<ToolAssignment>.Some(toolAssignment);
        }

        public async Task<IReadOnlyList<ToolAssignment>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolAssignments
                .AsNoTracking()
                .Where(ta => ta.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
