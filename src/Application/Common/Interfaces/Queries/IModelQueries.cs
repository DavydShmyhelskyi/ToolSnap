using Domain.Models.ToolInfo;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IModelQueries
    {
        Task<IReadOnlyList<Model>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Model>> GetByIdAsync(ModelId modelId, CancellationToken cancellationToken);
        Task<Option<Model>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<IReadOnlyList<Model>> GetByBrandIdAsync(BrandId brandId, CancellationToken cancellationToken);
    }
}