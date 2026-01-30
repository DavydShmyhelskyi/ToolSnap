using Domain.Models.Tools;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{

    public interface IToolsRepository
    {
        Task<Tool> AddAsync(Tool entity, CancellationToken cancellationToken);
        Task<Tool> UpdateAsync(Tool entity, CancellationToken cancellationToken);
        Task<Tool> DeleteAsync(Tool entity, CancellationToken cancellationToken);

    }

}