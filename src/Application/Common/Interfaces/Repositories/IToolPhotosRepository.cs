using Domain.Models.ToolPhotos;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolPhotosRepository
    {
        Task<ToolPhoto> AddAsync(ToolPhoto entity, CancellationToken cancellationToken);
        Task<ToolPhoto> DeleteAsync(ToolPhoto entity, CancellationToken cancellationToken);
    }
}
