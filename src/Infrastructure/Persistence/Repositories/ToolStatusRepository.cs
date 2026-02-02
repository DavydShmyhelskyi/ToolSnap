using Application.Common.Interfaces.Repositories;
using Domain.Models.Tools;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolStatusRepository : IToolStatusRepository
    {
        public Task<ToolStatus> AddAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ToolStatus> DeleteAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ToolStatus> UpdateAsync(ToolStatus entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
