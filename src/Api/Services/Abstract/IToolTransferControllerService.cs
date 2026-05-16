using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolTransferControllerService
    {
        Task<Option<ToolTransferDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}
