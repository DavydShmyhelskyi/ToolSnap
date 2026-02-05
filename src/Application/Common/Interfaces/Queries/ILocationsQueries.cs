using Domain.Models.Locations;
using LanguageExt;


namespace Application.Common.Interfaces.Queries
{
    public interface ILocationsQueries
    {
        Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken);
        Task<Option<Location>> GetByTitleAsync(string name, CancellationToken cancellationToken);
        Task<IReadOnlyList<Location>> GetByTypeAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken);
    }
}
