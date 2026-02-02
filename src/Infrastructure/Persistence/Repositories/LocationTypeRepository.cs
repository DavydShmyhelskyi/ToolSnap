using Application.Common.Interfaces.Repositories;
using Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LocationTypeRepository(ApplicationDbContext context) : ILocationTypeRepository
    {
        public async Task<LocationType> AddAsync(LocationType entity, CancellationToken cancellationToken)
        {
            await context.Set<LocationType>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<LocationType> DeleteAsync(LocationType entity, CancellationToken cancellationToken)
        {
            context.Set<LocationType>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<LocationType> UpdateAsync(LocationType entity, CancellationToken cancellationToken)
        {
            context.Set<LocationType>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}