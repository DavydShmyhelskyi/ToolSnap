using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolLiabilities;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolLiabilityRepository(ApplicationDbContext context) : IToolLiabilityRepository
    {
        public async Task<ToolLiability> AddAsync(ToolLiability entity, CancellationToken cancellationToken)
        {
            await context.ToolLiabilities.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<ToolLiability>> AddRangeAsync(
            IEnumerable<ToolLiability> entities,
            CancellationToken cancellationToken)
        {
            var list = entities.ToList();
            await context.ToolLiabilities.AddRangeAsync(list, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return list;
        }

        public async Task<ToolLiability> UpdateAsync(ToolLiability entity, CancellationToken cancellationToken)
        {
            context.ToolLiabilities.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<ToolLiability>> UpdateRangeAsync(
            IEnumerable<ToolLiability> entities,
            CancellationToken cancellationToken)
        {
            var list = entities.ToList();
            context.ToolLiabilities.UpdateRange(list);
            await context.SaveChangesAsync(cancellationToken);
            return list;
        }
    }
}
