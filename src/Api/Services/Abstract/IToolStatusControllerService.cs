using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolStatusControllerService
    {
        Task<Option<ToolStatusDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}