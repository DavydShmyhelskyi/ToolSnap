using Domain.Models.ToolInfo;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IBrandQueries
    {
        Task<IReadOnlyList<Brand>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Brand>> GetByIdAsync(BrandId brandId, CancellationToken cancellationToken);
        Task<Option<Brand>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}