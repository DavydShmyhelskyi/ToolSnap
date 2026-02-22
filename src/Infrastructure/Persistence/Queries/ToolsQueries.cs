using Application.Common.Interfaces.Queries;
using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using Domain.Models.Users;
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

        public async Task<IReadOnlyList<Tool>> GetAllByTypeAndModelAsync(ToolTypeId toolTypeId, BrandId? brandId, ModelId? modelId, CancellationToken cancellationToken)
        {
            var query = context.Tools
        .AsQueryable()
        .Where(t => t.ToolTypeId == toolTypeId);

            if (brandId is not null)
            {
                query = query.Where(t => t.BrandId == brandId);
            }

            if (modelId is not null)
            {
                query = query.Where(t => t.ModelId == modelId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Tool>> GetNotReturnedToolsByUserAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            return await context.ToolAssignments
                .Where(ta =>
                    ta.UserId == userId &&
                    ta.ReturnedAt == null &&
                    ta.ReturnedLocationId == null &&
                    ta.ReturnedDetectedToolId == null)
                .Include(ta => ta.Tool)
                .Select(ta => ta.Tool!)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Tool>> GetAllAvailableByTypeAndModelAsync(
           ToolTypeId toolTypeId,
           BrandId? brandId,
           ModelId? modelId,
           CancellationToken cancellationToken)
        {
            var query = context.Tools
                .AsQueryable()
                .Where(t => t.ToolTypeId == toolTypeId);

            if (brandId is not null)
            {
                query = query.Where(t => t.BrandId == brandId);
            }

            if (modelId is not null)
            {
                query = query.Where(t => t.ModelId == modelId);
            }

            // ❗ ВАЖЛИВО: виключаємо інструменти, які мають активне призначення
            query = query.Where(t =>
                !context.ToolAssignments.Any(ta =>
                    ta.ToolId == t.Id &&
                    ta.ReturnedAt == null &&
                    ta.ReturnedLocationId == null &&
                    ta.ReturnedDetectedToolId == null));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Tool>> GetNotReturnedToolsByUserAndTypeAndModelAsync(
            UserId userId,
            ToolTypeId toolTypeId,
            BrandId? brandId,
            ModelId? modelId,
            CancellationToken cancellationToken)
        {
            var query = context.ToolAssignments
                .Include(ta => ta.Tool)
                .Where(ta =>
                    ta.UserId == userId &&
                    ta.ReturnedAt == null &&
                    ta.ReturnedLocationId == null &&
                    ta.ReturnedDetectedToolId == null &&
                    ta.Tool != null &&
                    ta.Tool.ToolTypeId == toolTypeId);

            if (brandId is not null)
            {
                query = query.Where(ta => ta.Tool!.BrandId == brandId);
            }

            if (modelId is not null)
            {
                query = query.Where(ta => ta.Tool!.ModelId == modelId);
            }

            return await query
                .Select(ta => ta.Tool!)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Tool>> GetAnyToolsByTypeAndBrandAsync(
    ToolTypeId? toolTypeId,
    BrandId? brandId,
    CancellationToken cancellationToken)
        {
            var query = context.Tools.AsQueryable();

            if (toolTypeId is not null)
                query = query.Where(t => t.ToolTypeId == toolTypeId);

            if (brandId is not null)
                query = query.Where(t => t.BrandId == brandId);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
