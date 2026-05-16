using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.ToolTransfers.Exceptions;
using Domain.Models.Tools;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.ToolTransfers.Commands
{
    public record InitiateToolTransferCommand : IRequest<Either<ToolTransferException, ToolTransfer>>
    {
        public required Guid FromUserId { get; init; }
        public required Guid ToUserId { get; init; }
        public required Guid ToolId { get; init; }
    }

    public class InitiateToolTransferCommandHandler(
        IToolTransferRepository transferRepository,
        IToolTransferQueries transferQueries,
        IUsersQueries usersQueries,
        IToolsQueries toolsQueries,
        IToolAssignmentQueries assignmentQueries)
        : IRequestHandler<InitiateToolTransferCommand, Either<ToolTransferException, ToolTransfer>>
    {
        public async Task<Either<ToolTransferException, ToolTransfer>> Handle(
            InitiateToolTransferCommand request,
            CancellationToken cancellationToken)
        {
            var fromUserId = new UserId(request.FromUserId);
            var toUserId = new UserId(request.ToUserId);
            var toolId = new ToolId(request.ToolId);

            if (fromUserId == toUserId)
                return new ToolTransferSelfTransferException();

            var fromUser = await usersQueries.GetByIdAsync(fromUserId, cancellationToken);
            if (fromUser.IsNone)
                return new UserNotFoundForToolTransferException(fromUserId);

            var toUser = await usersQueries.GetByIdAsync(toUserId, cancellationToken);
            if (toUser.IsNone)
                return new UserNotFoundForToolTransferException(toUserId);

            var tool = await toolsQueries.GetByIdAsync(toolId, cancellationToken);
            if (tool.IsNone)
                return new ToolNotFoundForToolTransferException(toolId);

            var activeAssignment = await assignmentQueries.GetActiveByUserAndToolAsync(fromUserId, toolId, cancellationToken);
            if (activeAssignment.IsNone)
                return new ToolNotAssignedToUserException(toolId, fromUserId);

            var pendingTransfer = await transferQueries.GetPendingByToolIdAsync(toolId, cancellationToken);
            if (pendingTransfer.IsSome)
                return new ToolTransferPendingAlreadyExistsException(toolId);

            return await activeAssignment.MatchAsync(
                assignment => CreateTransfer(assignment.Id, toolId, fromUserId, toUserId, cancellationToken),
                () => Task.FromResult<Either<ToolTransferException, ToolTransfer>>(
                    new ToolNotAssignedToUserException(toolId, fromUserId)));
        }

        private async Task<Either<ToolTransferException, ToolTransfer>> CreateTransfer(
            Domain.Models.ToolAssignments.ToolAssignmentId assignmentId,
            ToolId toolId,
            UserId fromUserId,
            UserId toUserId,
            CancellationToken cancellationToken)
        {
            try
            {
                var transfer = ToolTransfer.New(toolId, assignmentId, fromUserId, toUserId);
                return await transferRepository.AddAsync(transfer, cancellationToken);
            }
            catch (Exception ex)
            {
                return new UnhandledToolTransferException(ToolTransferId.Empty(), ex);
            }
        }
    }
}
