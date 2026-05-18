using Domain.Models.Tools;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolTransferQueries
    {
        Task<IReadOnlyList<ToolTransfer>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolTransfer>> GetByIdAsync(ToolTransferId id, CancellationToken cancellationToken);
        Task<Option<ToolTransfer>> GetPendingByToolIdAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolTransfer>> GetAllByToolIdAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolTransfer>> GetAllByFromUserIdAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolTransfer>> GetPendingByToUserIdAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolTransfer>> GetAllByToUserIdAsync(UserId userId, CancellationToken cancellationToken);
    }
}
