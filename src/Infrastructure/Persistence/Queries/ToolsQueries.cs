using Application.Common.Interfaces.Queries;
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
            return await context.Tools
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<Tool>> GetByIdAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == toolId, cancellationToken);
            return tool == null ? Option<Tool>.None : Option<Tool>.Some(tool);
        }

        public async Task<IReadOnlyList<Tool>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            return await context.Tools
                .AsNoTracking()
                .Where(t => t.Id == toolId)
                .ToListAsync(cancellationToken);
        }

        /*public async Task<Option<Tool>> GetByTitleAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ToolTypeId == toolTypeId, cancellationToken);
            return tool == null ? Option<Tool>.None : Option<Tool>.Some(tool);
        }

        public async Task<Option<Tool>> GetByBrandAsync(BrandId brandId, CancellationToken cancellationToken)
        {
            var tool = await context.Tools
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.BrandId == brandId, cancellationToken);
            return tool == null ? Option<Tool>.None : Option<Tool>.Some(tool);
        }*/

        public Task<Option<Tool>> GetByToolTypeIdAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Option<Tool>> GetByBrandAsync(string brand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
