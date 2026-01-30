using Domain.Models.Locations;
using Domain.Models.Roles;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;
    public interface ILocationTypeRepository
{
    Task<LocationType> AddAsync(LocationType entity, CancellationToken cancellationToken);
    Task<LocationType> UpdateAsync(LocationType entity, CancellationToken cancellationToken);
    Task<LocationType> DeleteAsync(LocationType entity, CancellationToken cancellationToken);
}