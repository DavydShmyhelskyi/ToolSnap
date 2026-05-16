using Application.Common.Models;

namespace Application.Common.Interfaces.Queries
{
    public interface IOverdueReportQueries
    {
        Task<IReadOnlyList<OverdueAssignmentData>> GetOverdueAsync(CancellationToken cancellationToken);
    }
}
