using Application.Common.Interfaces.Queries;
using Domain.Models.ToolInfo;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolStatusQueries(ApplicationDbContext context) : IToolStatusQueries
    {
        public async Task<IReadOnlyList<ToolStatus>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolStatuses.ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolStatus>> GetByIdAsync(ToolStatusId toolStatusId, CancellationToken cancellationToken)
        {
            var toolStatus = await context.ToolStatuses.FirstOrDefaultAsync(ts => ts.Id == toolStatusId, cancellationToken);
            return toolStatus != null ? Option<ToolStatus>.Some(toolStatus) : Option<ToolStatus>.None;
        }

        public async Task<Option<ToolStatus>> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var toolStatus = await context.ToolStatuses.FirstOrDefaultAsync(ts => ts.Title == title, cancellationToken);
            return toolStatus != null ? Option<ToolStatus>.Some(toolStatus) : Option<ToolStatus>.None;
        }
    }
}
