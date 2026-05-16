using Application.Common.Interfaces.Queries;
using Domain.Models.ToolAssignments;
using Domain.Models.ToolLiabilities;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolLiabilityQueries(ApplicationDbContext context) : IToolLiabilityQueries
    {
        public async Task<IReadOnlyList<ToolLiability>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolLiabilities
                .OrderByDescending(x => x.AssignedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolLiability>> GetByIdAsync(ToolLiabilityId id, CancellationToken cancellationToken)
        {
            var liability = await context.ToolLiabilities
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return liability != null ? Option<ToolLiability>.Some(liability) : Option<ToolLiability>.None;
        }

        public async Task<Option<ToolLiability>> GetOpenByToolAssignmentIdAsync(
            ToolAssignmentId assignmentId,
            CancellationToken cancellationToken)
        {
            var liability = await context.ToolLiabilities
                .FirstOrDefaultAsync(
                    x => x.ToolAssignmentId == assignmentId && x.ClosedAt == null,
                    cancellationToken);
            return liability != null ? Option<ToolLiability>.Some(liability) : Option<ToolLiability>.None;
        }

        public async Task<IReadOnlyList<ToolLiability>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            return await context.ToolLiabilities
                .Where(x => x.ToolId == toolId)
                .OrderByDescending(x => x.AssignedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolLiability>> GetAllByWorkerAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolLiabilities
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AssignedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolLiability>> GetOpenByWorkerAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolLiabilities
                .Where(x => x.UserId == userId && x.ClosedAt == null)
                .OrderByDescending(x => x.AssignedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<(decimal TotalValue, int ToolCount)> GetInventoryStatsAsync(CancellationToken cancellationToken)
        {
            var stats = await context.Tools
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    TotalValue = g.Sum(t => t.Price),
                    ToolCount = g.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return stats is null ? (0m, 0) : (stats.TotalValue, stats.ToolCount);
        }

        public async Task<(decimal TotalValue, int ToolCount)> GetWorkerOnHandsStatsAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            var stats = await context.ToolLiabilities
                .Where(x => x.UserId == userId && x.ClosedAt == null)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    TotalValue = g.Sum(x => x.PriceAtAssignment),
                    ToolCount = g.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return stats is null ? (0m, 0) : (stats.TotalValue, stats.ToolCount);
        }
    }
}
