using Domain.Models.ToolAssignments;

namespace Api.DTOs
{
    public record ToolAssignmentDto(
        Guid Id,
        Guid ToolId,
        string? ToolName,
        Guid UserId,
        string? UserFullName,
        Guid LocationId,
        string? LocationName,
        DateTime TakenAt,
        DateTime? ReturnedAt,
        bool IsActive)
    {
        public static ToolAssignmentDto FromDomain(ToolAssignment toolAssignment) =>
            new(
                toolAssignment.Id.Value,
                toolAssignment.ToolId.Value,
                toolAssignment.Tool?.Name,
                toolAssignment.UserId.Value,
                toolAssignment.User?.FullName,
                toolAssignment.LocationId.Value,
                toolAssignment.Location?.Name,
                toolAssignment.TakenAt,
                toolAssignment.ReturnedAt,
                !toolAssignment.ReturnedAt.HasValue);
    }

    public record CreateToolAssignmentDto(
        Guid ToolId,
        Guid UserId,
        Guid LocationId);

    public record UpdateToolAssignmentDto(Guid LocationId);

    public record ReturnToolAssignmentDto();
}