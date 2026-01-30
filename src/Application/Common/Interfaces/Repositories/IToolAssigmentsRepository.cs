using Domain.Models.ToolAssignments;


namespace Application.Common.Interfaces.Repositories
{
    public interface IToolAssigmentsRepository
    {
        Task<ToolAssignment> Approve(CancellationToken cancellationToken);
    }
}
