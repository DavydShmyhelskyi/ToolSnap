using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolAssignmentControllerService
    {
        Task<Option<ToolAssignmentDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}