using Application.Common.Interfaces.Repositories;
using Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LocationRepository(ApplicationDbContext context) : ILocationRepository
    {
        public async Task<Location> AddAsync(Location entity, CancellationToken cancellationToken)
        {
            await context.Locations.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Location> DeleteAsync(Location entity, CancellationToken cancellationToken)
        {
            context.Locations.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Location> UpdateAsync(Location entity, CancellationToken cancellationToken)
        {
            context.Locations.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}