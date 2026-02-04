using Domain.Models.ToolInfo;

namespace Application.Common.Interfaces.Repositories
{
    public interface IModelRepository
    {
        Task<Model> AddAsync(Model entity, CancellationToken cancellationToken);
        Task<Model> UpdateAsync(Model entity, CancellationToken cancellationToken);
        Task<Model> DeleteAsync(Model entity, CancellationToken cancellationToken);
    }
}