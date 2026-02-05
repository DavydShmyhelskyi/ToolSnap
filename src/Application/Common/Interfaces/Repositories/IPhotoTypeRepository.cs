using Domain.Models.ToolPhotos;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPhotoTypeRepository
    {
        Task<PhotoType> AddAsync(PhotoType entity, CancellationToken cancellationToken);
        Task<PhotoType> UpdateAsync(PhotoType entity, CancellationToken cancellationToken);
        Task<PhotoType> DeleteAsync(PhotoType entity, CancellationToken cancellationToken);

    }
}
