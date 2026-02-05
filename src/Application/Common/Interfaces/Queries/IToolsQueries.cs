using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.Tools;
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
        Task<IReadOnlyList<Tool>> GetAllByTypeAndModelAsync(ToolTypeId toolTypeId, ModelId modelId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Tool>> GetAllAvailableToolsByTypeAndModelAsync(ToolAssignmentId lastToolAssignmentId, ToolTypeId toolTypeId, ModelId modelId, CancellationToken cancellationToken);
    }
}
