using Application.Common.Interfaces.Queries;
using Domain.Models.Locations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class LocationsQueries(ApplicationDbContext context) : ILocationsQueries
    {
        public async Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Locations
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Option<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken)
        {
            var location = await context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken);
            return location == null ? Option<Location>.None : Option<Location>.Some(location);
        }

        public async Task<Option<Location>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var location = await context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
            return location == null ? Option<Location>.None : Option<Location>.Some(location);
        }
    }
}
