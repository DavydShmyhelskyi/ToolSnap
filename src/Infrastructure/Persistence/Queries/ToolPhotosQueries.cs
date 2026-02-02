using Application.Common.Interfaces.Queries;
using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolPhotosQueries(ApplicationDbContext context) : IToolPhotosQueries
    {
        public async Task<IReadOnlyList<ToolPhoto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Set<ToolPhoto>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolPhoto>> GetByIdAsync(ToolPhotoId toolPhotoId, CancellationToken cancellationToken)
        {
            var toolPhoto = await context.Set<ToolPhoto>()
                .AsNoTracking()
                .FirstOrDefaultAsync(tp => tp.Id == toolPhotoId, cancellationToken);
            return toolPhoto == null ? Option<ToolPhoto>.None : Option<ToolPhoto>.Some(toolPhoto);
        }

        public async Task<IReadOnlyList<ToolPhoto>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            return await context.Set<ToolPhoto>()
                .AsNoTracking()
                .Where(tp => tp.ToolId == toolId)
                .ToListAsync(cancellationToken);
        }
    }
}
