using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IModelControllerService
    {
        Task<Option<ModelDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}