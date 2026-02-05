using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class BrandRepository(ApplicationDbContext context) : IBrandRepository
    {
        public async Task<Brand> AddAsync(Brand entity, CancellationToken cancellationToken)
        {
            await context.Brands.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Brand> UpdateAsync(Brand entity, CancellationToken cancellationToken)
        {
            context.Brands.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Brand> DeleteAsync(Brand entity, CancellationToken cancellationToken)
        {
            context.Brands.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}