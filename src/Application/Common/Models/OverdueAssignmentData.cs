using Domain.Models.ToolAssignments;

namespace Application.Common.Models
{
    public record OverdueAssignmentData(ToolAssignment Assignment, decimal ValueAtRisk);
}
