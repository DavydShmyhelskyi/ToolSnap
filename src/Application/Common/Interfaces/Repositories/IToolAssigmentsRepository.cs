using Domain.Models.ToolAssignments;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolAssignmentsRepository
    {
        Task<ToolAssignment> AddAsync(ToolAssignment entity, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> AddRangeAsync(IEnumerable<ToolAssignment> entities, CancellationToken cancellationToken);

        Task<ToolAssignment> UpdateAsync(ToolAssignment entity, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolAssignment>> UpdateRangeAsync(IEnumerable<ToolAssignment> entities, CancellationToken cancellationToken);

        Task<ToolAssignment> DeleteAsync(ToolAssignment entity, CancellationToken cancellationToken);
    }
}
