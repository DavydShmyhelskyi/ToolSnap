using Domain.Models.Locations;
using Domain.Models.Roles;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;
    public interface ILocationTypeRepository
{
    Task<LocationType> AddAsync(LocationType entity, CancellationToken cancellationToken);
    Task<LocationType> UpdateAsync(LocationType entity, CancellationToken cancellationToken);
    Task<LocationType> DeleteAsync(LocationType entity, CancellationToken cancellationToken);

    Task<Option<LocationType>> GetByIdAsync(LocationTypeId id, CancellationToken cancellationToken);
    Task<Option<LocationType>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<LocationType>> GetAllAsync(CancellationToken cancellationToken);
}