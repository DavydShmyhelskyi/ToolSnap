using Application.Common.Interfaces.Repositories;
using Domain.Models.Locations;

namespace Infrastructure.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository

    {
        public Task<Location> AddAsync(Location entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Location> DeleteAsync(Location entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Location> UpdateAsync(Location entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
