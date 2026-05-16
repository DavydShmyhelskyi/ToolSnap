using Domain.Models.ToolTransfers;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolTransferRepository
    {
        Task<ToolTransfer> AddAsync(ToolTransfer entity, CancellationToken cancellationToken);
        Task<ToolTransfer> UpdateAsync(ToolTransfer entity, CancellationToken cancellationToken);
        Task<ToolTransfer> DeleteAsync(ToolTransfer entity, CancellationToken cancellationToken);
    }
}
