using Domain.Models.PhotoSessions;

namespace Application.Common.Interfaces.Repositories
{
    public interface IActionTypeRepository
    {
        Task<ActionType> AddAsync(ActionType entity, CancellationToken cancellationToken);
        Task<ActionType> UpdateAsync(ActionType entity, CancellationToken cancellationToken);
        Task<ActionType> DeleteAsync(ActionType entity, CancellationToken cancellationToken);
    }
}
