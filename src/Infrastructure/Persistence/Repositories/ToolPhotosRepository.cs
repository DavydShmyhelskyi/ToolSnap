using Application.Common.Interfaces.Repositories;
using Domain.Models.ToolPhotos;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolPhotosRepository : IToolPhotosRepository
    {
        public Task<ToolPhoto> AddAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ToolPhoto> DeleteAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ToolPhoto> UpdateAsync(ToolPhoto entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
