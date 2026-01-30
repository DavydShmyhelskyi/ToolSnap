using Domain.Models.Tools;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{

    public interface IToolsRepository
    {
        Task<Tool> AddAsync(Tool entity, CancellationToken cancellationToken);
        Task<Tool> UpdateAsync(Tool entity, CancellationToken cancellationToken);
        Task<Tool> DeleteAsync(Tool entity, CancellationToken cancellationToken);

        Task<Option<Tool>> GetByIdAsync(Tool id, CancellationToken cancellationToken);
        Task<Option<Tool>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllAsync(CancellationToken cancellationToken);
    }

}