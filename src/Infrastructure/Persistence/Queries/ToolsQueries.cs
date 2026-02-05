using Application.Common.Interfaces.Queries;
using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolsQueries(ApplicationDbContext context) : IToolsQueries
    {
        public async Task<IReadOnlyList<Tool>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Tools.ToListAsync(cancellationToken);
        }

        public async Task<Option<Tool>> GetByIdAsync(ToolId toolPhotoId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools.FirstOrDefaultAsync(t => t.Id == toolPhotoId, cancellationToken);
            return tool != null ? Option<Tool>.Some(tool) : Option<Tool>.None;
        }

        public async Task<IReadOnlyList<Tool>> GetAllByStatusIdAsync(ToolStatusId toolStatusId, CancellationToken cancellationToken)
        {
            return await context.Tools.Where(t => t.ToolStatusId == toolStatusId).ToListAsync(cancellationToken);
        }

        public async Task<Option<Tool>> GetByTypeAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools.FirstOrDefaultAsync(t => t.ToolTypeId == toolTypeId, cancellationToken);
            return tool != null ? Option<Tool>.Some(tool) : Option<Tool>.None;
        }

        public async Task<Option<Tool>> GetByBrandAsync(BrandId brandId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools.FirstOrDefaultAsync(t => t.BrandId == brandId, cancellationToken);
            return tool != null ? Option<Tool>.Some(tool) : Option<Tool>.None;
        }

        public async Task<IReadOnlyList<Tool>> GetAllByTypeAndModelAsync(ToolTypeId toolTypeId, ModelId modelId, CancellationToken cancellationToken)
        {
            return await context.Tools.Where(t => t.ToolTypeId == toolTypeId && t.ModelId == modelId).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Tool>> GetAllAvailableToolsByTypeAndModelAsync(ToolAssignmentId lastToolAssignmentId, ToolTypeId toolTypeId, ModelId modelId, CancellationToken cancellationToken)
        {
            return await context.Tools.Where(t => t.ToolTypeId == toolTypeId && t.ModelId == modelId).ToListAsync(cancellationToken);
        }
    }
}
