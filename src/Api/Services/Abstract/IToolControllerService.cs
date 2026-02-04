using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolControllerService
    {
        Task<Option<ToolDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}