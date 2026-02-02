using Application.Common.Interfaces.Queries;
using Domain.Models.Locations;
using Domain.Models.Tools;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolStatusQueries(ApplicationDbContext context) : IToolStatusQueries
    {
        public async Task<IReadOnlyList<ToolStatus>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Set<ToolStatus>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolStatus>> GetByIdAsync(ToolStatusId toolStatusId, CancellationToken cancellationToken)
        {
            var toolStatus = await context.Set<ToolStatus>()
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.Id == toolStatusId, cancellationToken);
            return toolStatus == null ? Option<ToolStatus>.None : Option<ToolStatus>.Some(toolStatus);
        }

        public Task<Option<LocationType>> GetByIdAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Option<ToolStatus>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var toolStatus = await context.Set<ToolStatus>()
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.Title == name, cancellationToken);
            return toolStatus == null ? Option<ToolStatus>.None : Option<ToolStatus>.Some(toolStatus);
        }

        Task<IReadOnlyList<LocationType>> IToolStatusQueries.GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Option<LocationType>> IToolStatusQueries.GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
