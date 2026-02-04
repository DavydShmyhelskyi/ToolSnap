using Domain.Models.ToolInfo;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolTypeRepository
    {
        Task<ToolType> AddAsync(ToolType entity, CancellationToken cancellationToken);
        Task<ToolType> UpdateAsync(ToolType entity, CancellationToken cancellationToken);
        Task<ToolType> DeleteAsync(ToolType entity, CancellationToken cancellationToken);
    }
}