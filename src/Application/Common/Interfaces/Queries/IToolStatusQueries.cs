using Domain.Models.ToolInfo;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolStatusQueries
    {
        Task<IReadOnlyList<ToolStatus>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolStatus>> GetByIdAsync(ToolStatusId toolStatusId, CancellationToken cancellationToken);
        Task<Option<ToolStatus>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}
