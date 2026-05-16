using Domain.Models.ToolLiabilities;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolLiabilityRepository
    {
        Task<ToolLiability> AddAsync(ToolLiability entity, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolLiability>> AddRangeAsync(IEnumerable<ToolLiability> entities, CancellationToken cancellationToken);
        Task<ToolLiability> UpdateAsync(ToolLiability entity, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolLiability>> UpdateRangeAsync(IEnumerable<ToolLiability> entities, CancellationToken cancellationToken);
    }
}
