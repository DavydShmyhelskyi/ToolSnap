using Domain.Models.ToolInfo;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolTypeQueries
    {
        Task<IReadOnlyList<ToolType>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolType>> GetByIdAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken);
        Task<Option<ToolType>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}