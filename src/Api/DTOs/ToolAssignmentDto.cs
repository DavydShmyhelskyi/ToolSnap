using Domain.Models.ToolAssignments;

namespace Api.DTOs
{
    public record ToolAssignmentDto(
        Guid Id,
        Guid TakenDetectedToolId,
        Guid? ReturnedDetectedToolId,
        Guid ToolId,
        Guid UserId,
        Guid TakenLocationId,
        Guid? ReturnedLocationId,
        DateTime TakenAt,
        DateTime? ReturnedAt)
    {
        public static ToolAssignmentDto FromDomain(ToolAssignment toolAssignment) =>
            new(
                toolAssignment.Id.Value,
                toolAssignment.TakenDetectedToolId.Value,
                toolAssignment.ReturnedDetectedToolId?.Value,
                toolAssignment.ToolId.Value,
                toolAssignment.UserId.Value,
                toolAssignment.TakenLocationId.Value,
                toolAssignment.ReturnedLocationId?.Value,
                toolAssignment.TakenAt,
                toolAssignment.ReturnedAt);
    }

    public record CreateToolAssignmentDto(
        Guid TakenDetectedToolId,
        Guid ToolId,
        Guid UserId,
        Guid TakenLocationId);

    public record ReturnToolAssignmentDto(
        Guid ReturnedLocationId,
        Guid ReturnedDetectedToolId);
}