using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolPhotos;

namespace Infrastructure.Persistence.Repositories
{
    public class PhotoTypeRepository : IPhotoTypeRepository
    {
        public Task<PhotoType> AddAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PhotoType> DeleteAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PhotoType> UpdateAsync(PhotoType entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
