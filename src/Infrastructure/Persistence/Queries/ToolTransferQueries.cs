using Application.Common.Interfaces.Queries;
using Domain.Models.Tools;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class ToolTransferQueries(ApplicationDbContext context) : IToolTransferQueries
    {
        public async Task<IReadOnlyList<ToolTransfer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ToolTransfers.ToListAsync(cancellationToken);
        }

        public async Task<Option<ToolTransfer>> GetByIdAsync(ToolTransferId id, CancellationToken cancellationToken)
        {
            var transfer = await context.ToolTransfers
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            return transfer != null ? Option<ToolTransfer>.Some(transfer) : Option<ToolTransfer>.None;
        }

        public async Task<Option<ToolTransfer>> GetPendingByToolIdAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            var transfer = await context.ToolTransfers
                .FirstOrDefaultAsync(
                    t => t.ToolId == toolId && t.Status == ToolTransferStatus.Pending,
                    cancellationToken);
            return transfer != null ? Option<ToolTransfer>.Some(transfer) : Option<ToolTransfer>.None;
        }

        public async Task<IReadOnlyList<ToolTransfer>> GetAllByToolIdAsync(ToolId toolId, CancellationToken cancellationToken)
        {
            return await context.ToolTransfers
                .Where(t => t.ToolId == toolId)
                .OrderByDescending(t => t.InitiatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolTransfer>> GetAllByFromUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolTransfers
                .Where(t => t.FromUserId == userId)
                .OrderByDescending(t => t.InitiatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolTransfer>> GetPendingByToUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolTransfers
                .Where(t => t.ToUserId == userId && t.Status == ToolTransferStatus.Pending)
                .OrderByDescending(t => t.InitiatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ToolTransfer>> GetAllByToUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            return await context.ToolTransfers
                .Where(t => t.ToUserId == userId)
                .OrderByDescending(t => t.InitiatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
