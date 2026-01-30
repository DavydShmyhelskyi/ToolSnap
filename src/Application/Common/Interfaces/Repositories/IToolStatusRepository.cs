using Domain.Models.Roles;
using Domain.Models.Tools;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IToolStatusRepository
    {
        Task<ToolStatus> AddAsync(ToolStatus entity, CancellationToken cancellationToken);
        Task<ToolStatus> UpdateAsync(ToolStatus entity, CancellationToken cancellationToken);
        Task<ToolStatus> DeleteAsync(ToolStatus entity, CancellationToken cancellationToken);

        Task<Option<ToolStatus>> GetByIdAsync(ToolStatusId id, CancellationToken cancellationToken);
        Task<Option<ToolStatus>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IReadOnlyList<ToolStatus>> GetAllAsync(CancellationToken cancellationToken);
    }
}
