using Domain.Models.Locations;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Application.Common.Interfaces.Queries
{
    public interface ILocationsQueries
    {
        Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken);
        Task<Option<Location>> GetByTitleAsync(string name, CancellationToken cancellationToken);
    }
}
