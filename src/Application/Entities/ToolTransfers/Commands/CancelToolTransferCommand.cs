using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTransfers.Exceptions;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTransfers.Commands
{
    public record CancelToolTransferCommand : IRequest<Either<ToolTransferException, ToolTransfer>>
    {
        public required Guid TransferId { get; init; }
        public required Guid FromUserId { get; init; }
    }

    public class CancelToolTransferCommandHandler(
        IToolTransferQueries transferQueries,
        IToolTransferRepository transferRepository)
        : IRequestHandler<CancelToolTransferCommand, Either<ToolTransferException, ToolTransfer>>
    {
        public async Task<Either<ToolTransferException, ToolTransfer>> Handle(
            CancelToolTransferCommand request,
            CancellationToken cancellationToken)
        {
            var transferId = new ToolTransferId(request.TransferId);
            var fromUserId = new UserId(request.FromUserId);

            var transfer = await transferQueries.GetByIdAsync(transferId, cancellationToken);
            if (transfer.IsNone)
                return new ToolTransferNotFoundException(transferId);

            return await transfer.MatchAsync(
                t => CancelTransfer(t, fromUserId, cancellationToken),
                () => Task.FromResult<Either<ToolTransferException, ToolTransfer>>(
                    new ToolTransferNotFoundException(transferId)));
        }

        private async Task<Either<ToolTransferException, ToolTransfer>> CancelTransfer(
            ToolTransfer transfer,
            UserId requestingUserId,
            CancellationToken cancellationToken)
        {
            if (transfer.Status != ToolTransferStatus.Pending)
                return new ToolTransferNotPendingException(transfer.Id);

            if (transfer.FromUserId != requestingUserId)
                return new ToolTransferUnauthorizedException(transfer.Id);

            try
            {
                transfer.Cancel();
                return await transferRepository.UpdateAsync(transfer, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolTransferException(transfer.Id, ex);
            }
        }
    }
}
