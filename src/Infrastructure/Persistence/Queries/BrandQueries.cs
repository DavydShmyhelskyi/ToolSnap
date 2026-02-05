using Application.Common.Interfaces.Queries;
using Domain.Models.ToolInfo;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries
{
    public class BrandQueries(ApplicationDbContext context) : IBrandQueries
    {
        public async Task<IReadOnlyList<Brand>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Brands.ToListAsync(cancellationToken);
        }

        public async Task<Option<Brand>> GetByIdAsync(BrandId brandId, CancellationToken cancellationToken)
        {
            var brand = await context.Brands.FirstOrDefaultAsync(b => b.Id == brandId, cancellationToken);
            return brand != null ? Option<Brand>.Some(brand) : Option<Brand>.None;
        }

        public async Task<Option<Brand>> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var brand = await context.Brands.FirstOrDefaultAsync(b => b.Title == title, cancellationToken);
            return brand != null ? Option<Brand>.Some(brand) : Option<Brand>.None;
        }
    }
}