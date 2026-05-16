using Domain.Models.ToolTransfers;

namespace Api.DTOs
{
    public record ToolTransferDto(
        Guid Id,
        Guid ToolId,
        Guid ToolAssignmentId,
        Guid FromUserId,
        Guid ToUserId,
        string Status,
        DateTime InitiatedAt,
        DateTime? RespondedAt)
    {
        public static ToolTransferDto FromDomain(ToolTransfer transfer) =>
            new(
                transfer.Id.Value,
                transfer.ToolId.Value,
                transfer.ToolAssignmentId.Value,
                transfer.FromUserId.Value,
                transfer.ToUserId.Value,
                transfer.Status.ToString(),
                transfer.InitiatedAt,
                transfer.RespondedAt);
    }

    public record InitiateToolTransferDto(
        Guid FromUserId,
        Guid ToUserId,
        Guid ToolId);

    public record RespondToToolTransferDto(Guid ResponderUserId);

    public record CancelToolTransferDto(Guid InitiatorUserId);
}
