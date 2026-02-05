using Application.Common.Interfaces.Queries;
using Domain.Models.ToolInfo;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolTypeQueries(ApplicationDbContext context) : IToolTypeQueries
    {
        public async Task<IReadOnlyList<ToolType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolTypes.ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolType>> GetByIdAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken)
        {
            var toolType = await context.ToolTypes.FirstOrDefaultAsync(tt => tt.Id == toolTypeId, cancellationToken);
            return toolType != null ? Option<ToolType>.Some(toolType) : Option<ToolType>.None;
        }

        public async Task<Option<ToolType>> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var toolType = await context.ToolTypes.FirstOrDefaultAsync(tt => tt.Title == title, cancellationToken);
            return toolType != null ? Option<ToolType>.Some(toolType) : Option<ToolType>.None;
        }
    }
}