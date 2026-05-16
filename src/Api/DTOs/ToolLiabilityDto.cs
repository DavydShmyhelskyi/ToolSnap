using Domain.Models.ToolLiabilities;

namespace Api.DTOs
{
    public record ToolLiabilityDto(
        Guid Id,
        Guid ToolId,
        Guid ToolAssignmentId,
        Guid UserId,
        decimal PriceAtAssignment,
        DateTime AssignedAt,
        DateTime? ClosedAt,
        bool IsOpen)
    {
        public static ToolLiabilityDto FromDomain(ToolLiability liability) =>
            new(
                liability.Id.Value,
                liability.ToolId.Value,
                liability.ToolAssignmentId.Value,
                liability.UserId.Value,
                liability.PriceAtAssignment,
                liability.AssignedAt,
                liability.ClosedAt,
                liability.IsOpen);
    }

    public record InventoryStatsDto(decimal TotalValue, int ToolCount);

    public record WorkerOnHandsStatsDto(Guid UserId, decimal TotalValue, int ToolCount);
}
