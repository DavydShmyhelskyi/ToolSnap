using Domain.Models.ToolAssignments;


namespace Application.Common.Interfaces.Repositories
{
    public interface IToolAssigmentsRepository
    {
        Task<ToolAssignment> AddAsync(ToolAssignment entity, CancellationToken cancellationToken);
        Task<ToolAssignment> DeleteAsync(ToolAssignment entity, CancellationToken cancellationToken);
    }
}
