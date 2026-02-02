using Application.Common.Interfaces.Repositories;
using Domain.Models.Tools;

namespace Infrastructure.Persistence.Repositories
{
    public class ToolsRepository : IToolsRepository
    {
        public Task<Tool> AddAsync(Tool entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tool> DeleteAsync(Tool entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tool> UpdateAsync(Tool entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
