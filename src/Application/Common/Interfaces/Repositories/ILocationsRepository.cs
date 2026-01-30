using Domain.Models.Locations;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface ILocationRepository

    {
        Task<Location> AddAsync(Location entity, CancellationToken cancellationToken);
        Task<Location> UpdateAsync(Location entity, CancellationToken cancellationToken);
        Task<Location> DeleteAsync(Location entity, CancellationToken cancellationToken);
         
    }
}
