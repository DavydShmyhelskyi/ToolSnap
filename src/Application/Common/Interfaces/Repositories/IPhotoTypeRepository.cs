using Domain.Models.Roles;
using Domain.Models.ToolPhotos;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IPhotoTypeRepository
    {
        Task<PhotoType> AddAsync(PhotoType entity, CancellationToken cancellationToken);
        Task<PhotoType> UpdateAsync(PhotoType entity, CancellationToken cancellationToken);
        Task<PhotoType> DeleteAsync(PhotoType entity, CancellationToken cancellationToken);

        Task<Option<PhotoType>> GetByIdAsync(PhotoTypeId id, CancellationToken cancellationToken);
        Task<Option<PhotoType>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IReadOnlyList<PhotoType>> GetAllAsync(CancellationToken cancellationToken);
    }
}
