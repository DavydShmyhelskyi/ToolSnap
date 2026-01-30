using Domain.Models.Locations;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolStatusQueries
    {
        Task<IReadOnlyList<LocationType>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<LocationType>> GetByIdAsync(LocationTypeId locationTypeId, CancellationToken cancellationToken);
        Task<Option<LocationType>> GetByTitleAsync(string name, CancellationToken cancellationToken);
    }
}
