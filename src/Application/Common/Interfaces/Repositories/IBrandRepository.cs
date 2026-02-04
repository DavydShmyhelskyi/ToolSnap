using Domain.Models.ToolInfo;

namespace Application.Common.Interfaces.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> AddAsync(Brand entity, CancellationToken cancellationToken);
        Task<Brand> UpdateAsync(Brand entity, CancellationToken cancellationToken);
        Task<Brand> DeleteAsync(Brand entity, CancellationToken cancellationToken);
    }
}