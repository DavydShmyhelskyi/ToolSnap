using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTransfers.Exceptions;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTransfers.Commands
{
    public record RejectToolTransferCommand : IRequest<Either<ToolTransferException, ToolTransfer>>
    {
        public required Guid TransferId { get; init; }
        public required Guid ToUserId { get; init; }
    }

    public class RejectToolTransferCommandHandler(
        IToolTransferQueries transferQueries,
        IToolTransferRepository transferRepository)
        : IRequestHandler<RejectToolTransferCommand, Either<ToolTransferException, ToolTransfer>>
    {
        public async Task<Either<ToolTransferException, ToolTransfer>> Handle(
            RejectToolTransferCommand request,
            CancellationToken cancellationToken)
        {
            var transferId = new ToolTransferId(request.TransferId);
            var toUserId = new UserId(request.ToUserId);

            var transfer = await transferQueries.GetByIdAsync(transferId, cancellationToken);
            if (transfer.IsNone)
                return new ToolTransferNotFoundException(transferId);

            return await transfer.MatchAsync(
                t => RejectTransfer(t, toUserId, cancellationToken),
                () => Task.FromResult<Either<ToolTransferException, ToolTransfer>>(
                    new ToolTransferNotFoundException(transferId)));
        }

        private async Task<Either<ToolTransferException, ToolTransfer>> RejectTransfer(
            ToolTransfer transfer,
            UserId requestingUserId,
            CancellationToken cancellationToken)
        {
            if (transfer.Status != ToolTransferStatus.Pending)
                return new ToolTransferNotPendingException(transfer.Id);

            if (transfer.ToUserId != requestingUserId)
                return new ToolTransferUnauthorizedException(transfer.Id);

            try
            {
                transfer.Reject();
                return await transferRepository.UpdateAsync(transfer, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolTransferException(transfer.Id, ex);
            }
        }
    }
}
