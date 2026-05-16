using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTransfers.Exceptions;
using Domain.Models.ToolAssignments;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTransfers.Commands
{
    public record AcceptToolTransferCommand : IRequest<Either<ToolTransferException, ToolTransfer>>
    {
        public required Guid TransferId { get; init; }
        public required Guid ToUserId { get; init; }
    }

    public class AcceptToolTransferCommandHandler(
        IToolTransferQueries transferQueries,
        IToolTransferRepository transferRepository,
        IToolAssignmentQueries assignmentQueries,
        IToolAssignmentsRepository assignmentRepository,
        IApplicationDbContext dbContext)
        : IRequestHandler<AcceptToolTransferCommand, Either<ToolTransferException, ToolTransfer>>
    {
        public async Task<Either<ToolTransferException, ToolTransfer>> Handle(
            AcceptToolTransferCommand request,
            CancellationToken cancellationToken)
        {
            var transferId = new ToolTransferId(request.TransferId);
            var toUserId = new UserId(request.ToUserId);

            var transfer = await transferQueries.GetByIdAsync(transferId, cancellationToken);
            if (transfer.IsNone)
                return new ToolTransferNotFoundException(transferId);

            return await transfer.MatchAsync(
                t => AcceptTransfer(t, toUserId, cancellationToken),
                () => Task.FromResult<Either<ToolTransferException, ToolTransfer>>(
                    new ToolTransferNotFoundException(transferId)));
        }

        private async Task<Either<ToolTransferException, ToolTransfer>> AcceptTransfer(
            ToolTransfer transfer,
            UserId requestingUserId,
            CancellationToken cancellationToken)
        {
            if (transfer.Status != ToolTransferStatus.Pending)
                return new ToolTransferNotPendingException(transfer.Id);

            if (transfer.ToUserId != requestingUserId)
                return new ToolTransferUnauthorizedException(transfer.Id);

            var assignment = await assignmentQueries.GetByIdAsync(transfer.ToolAssignmentId, cancellationToken);
            if (assignment.IsNone)
                return new ToolAssignmentNotFoundForToolTransferException(transfer.ToolAssignmentId);

            using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var updated = await assignment.MatchAsync(
                    async a =>
                    {
                        a.Transfer(transfer.ToUserId);
                        await assignmentRepository.UpdateAsync(a, cancellationToken);

                        transfer.Accept();
                        return await transferRepository.UpdateAsync(transfer, cancellationToken);
                    },
                    () => Task.FromResult<ToolTransfer>(null!));

                transaction.Commit();
                return updated;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new UnhandledToolTransferException(transfer.Id, ex);
            }
        }
    }
}
