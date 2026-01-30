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


    }
}
