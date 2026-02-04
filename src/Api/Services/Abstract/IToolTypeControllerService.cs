using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolTypeControllerService
    {
        Task<Option<ToolTypeDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}