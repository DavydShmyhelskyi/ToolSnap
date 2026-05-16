using Application.Common.Interfaces.Queries;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class OverdueReportQueries(ApplicationDbContext context) : IOverdueReportQueries
    {
        public async Task<IReadOnlyList<OverdueAssignmentData>> GetOverdueAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var rows = await (
                from ta in context.ToolAssignments
                where ta.DueAt != null && ta.DueAt < now && ta.ReturnedAt == null
                join l in context.ToolLiabilities.Where(l => l.ClosedAt == null)
                    on ta.Id equals l.ToolAssignmentId into liabilities
                from l in liabilities.DefaultIfEmpty()
                orderby ta.DueAt
                select new
                {
                    Assignment = ta,
                    ValueAtRisk = l == null ? 0m : l.PriceAtAssignment
                }
            ).ToListAsync(cancellationToken);

            return rows
                .Select(r => new OverdueAssignmentData(r.Assignment, r.ValueAtRisk))
                .ToList();
        }
    }
}
