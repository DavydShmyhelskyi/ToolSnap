using Domain.Models.PhotoSessions;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IActionTypeQueries
    {
        Task<IReadOnlyList<ActionType>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ActionType>> GetByIdAsync(ActionTypeId actionTypeId, CancellationToken cancellationToken);
        Task<Option<ActionType>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}
