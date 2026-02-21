using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using Domain.Models.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IToolsQueries
    {
        Task<IReadOnlyList<Tool>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<Tool>> GetByIdAsync(ToolId toolPhotoId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllByStatusIdAsync(ToolStatusId toolStatusId, CancellationToken cancellationToken);
        Task<Option<Tool>> GetByTypeAsync(ToolTypeId toolTypeId, CancellationToken cancellationToken);
        Task<Option<Tool>> GetByBrandAsync(BrandId brandId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllByTypeAndModelAsync(ToolTypeId toolTypeId, BrandId? brandId, ModelId? modelId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetNotReturnedToolsByUserAsync(UserId userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllAvailableByTypeAndModelAsync(ToolTypeId toolTypeId, BrandId? brandId, ModelId? modelId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetNotReturnedToolsByUserAndTypeAndModelAsync(UserId userId, ToolTypeId toolTypeId, BrandId? brandId, ModelId? modelId, CancellationToken cancellationToken);
    }
}
