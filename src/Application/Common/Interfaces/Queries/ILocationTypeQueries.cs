using LanguageExt;
using Domain.Models.Locations;

namespace Application.Common.Interfaces.Queries
{
    public interface ILocationTypeQueries
    {
        Task<IReadOnlyList<LocationType>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<LocationType>> GetByIdAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken);
        Task<Option<LocationType>> GetByTitleAsync(string name, CancellationToken cancellationToken);
    }
}