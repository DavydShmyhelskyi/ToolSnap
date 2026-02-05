using Application.Common.Interfaces.Queries;
using Domain.Models.Locations;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class LocationTypeQueries(ApplicationDbContext context) : ILocationTypeQueries
    {
        public async Task<IReadOnlyList<LocationType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.LocationTypes.ToListAsync(cancellationToken);
        }

        public async Task<Option<LocationType>> GetByIdAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken)
        {
            var locationType = await context.LocationTypes.FirstOrDefaultAsync(lt => lt.Id == locationTypeId, cancellationToken);
            return locationType != null ? Option<LocationType>.Some(locationType) : Option<LocationType>.None;
        }

        public async Task<Option<LocationType>> GetByTitleAsync(string name, CancellationToken cancellationToken)
        {
            var locationType = await context.LocationTypes.FirstOrDefaultAsync(lt => lt.Title == name, cancellationToken);
            return locationType != null ? Option<LocationType>.Some(locationType) : Option<LocationType>.None;
        }
    }
}