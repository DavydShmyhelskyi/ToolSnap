using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolTransfers;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolTransferRepository(ApplicationDbContext context) : IToolTransferRepository
    {
        public async Task<ToolTransfer> AddAsync(ToolTransfer entity, CancellationToken cancellationToken)
        {
            await context.ToolTransfers.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolTransfer> UpdateAsync(ToolTransfer entity, CancellationToken cancellationToken)
        {
            context.ToolTransfers.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ToolTransfer> DeleteAsync(ToolTransfer entity, CancellationToken cancellationToken)
        {
            context.ToolTransfers.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
