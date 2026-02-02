using Application.Common.Interfaces.Repositories;
using Domain.Models.Locations;

namespace Infrastructure.Persistence.Repositories
{
    public class LocationTypeRepository : ILocationTypeRepository
    {
        public Task<LocationType> AddAsync(LocationType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<LocationType> DeleteAsync(LocationType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<LocationType> UpdateAsync(LocationType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}