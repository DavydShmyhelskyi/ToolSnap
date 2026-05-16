using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.ToolLiabilities;
using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolLiabilityQueries
    {
        Task<IReadOnlyList<ToolLiability>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<ToolLiability>> GetByIdAsync(ToolLiabilityId id, CancellationToken cancellationToken);
        Task<Option<ToolLiability>> GetOpenByToolAssignmentIdAsync(ToolAssignmentId assignmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolLiability>> GetAllByToolAsync(ToolId toolId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolLiability>> GetAllByWorkerAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolLiability>> GetOpenByWorkerAsync(UserId userId, CancellationToken cancellationToken);
        Task<(decimal TotalValue, int ToolCount)> GetInventoryStatsAsync(CancellationToken cancellationToken);
        Task<(decimal TotalValue, int ToolCount)> GetWorkerOnHandsStatsAsync(UserId userId, CancellationToken cancellationToken);
    }
}
