using Domain.Models.ToolPhotos;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolPhotosRepository
    {
        Task<ToolPhoto> AddAsync(ToolPhoto entity, CancellationToken cancellationToken);
        Task<ToolPhoto> UpdateAsync(ToolPhoto entity, CancellationToken cancellationToken);
        Task<ToolPhoto> DeleteAsync(ToolPhoto entity, CancellationToken cancellationToken);

        Task<Option<ToolPhoto>> GetByIdAsync(ToolPhoto id, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolPhoto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
