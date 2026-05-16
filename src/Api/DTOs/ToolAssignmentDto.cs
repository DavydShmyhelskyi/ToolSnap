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
        DateTime? ReturnedAt,
        DateTime? DueAt,
        bool IsOverdue)
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
                toolAssignment.ReturnedAt,
                toolAssignment.DueAt,
                toolAssignment.IsOverdue);
    }

    public record CreateToolAssignmentDto(
        Guid TakenDetectedToolId,
        Guid ToolId,
        Guid UserId,
        Guid TakenLocationId,
        DateTime? DueAt = null);

    public record ReturnToolAssignmentDto(
        Guid ReturnedLocationId,
        Guid ReturnedDetectedToolId);

    public record CreateToolAssignmentsBatchItemDto(
        Guid TakenDetectedToolId,
        Guid ToolId,
        Guid UserId,
        Guid LocationId,
        DateTime? DueAt = null);

    public record CreateToolAssignmentsBatchDto(
        IReadOnlyList<CreateToolAssignmentsBatchItemDto> Items);

    public record ReturnToolAssignmentsBatchItemDto(
        Guid ToolAssignmentId,
        Guid LocationId,
        Guid ReturnedDetectedToolId);

    public record ReturnToolAssignmentsBatchDto(
        IReadOnlyList<ReturnToolAssignmentsBatchItemDto> Items);

    public record SetToolAssignmentDueDto(DateTime? DueAt);
}