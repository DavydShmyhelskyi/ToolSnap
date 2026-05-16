namespace Api.DTOs
{
    public record OverdueAssignmentItemDto(
        Guid AssignmentId,
        Guid ToolId,
        DateTime DueAt,
        long OverdueMinutes,
        decimal ValueAtRisk);

    public record WorkerOverdueReportDto(
        Guid UserId,
        int OverdueCount,
        decimal TotalValueAtRisk,
        IReadOnlyList<OverdueAssignmentItemDto> Assignments);
}
