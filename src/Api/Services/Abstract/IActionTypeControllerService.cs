using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IActionTypeControllerService
    {
        Task<Option<ActionTypeDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}