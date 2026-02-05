using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ModelRepository(ApplicationDbContext context) : IModelRepository
    {
        public async Task<Model> AddAsync(Model entity, CancellationToken cancellationToken)
        {
            await context.Models.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Model> UpdateAsync(Model entity, CancellationToken cancellationToken)
        {
            context.Models.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Model> DeleteAsync(Model entity, CancellationToken cancellationToken)
        {
            context.Models.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}