using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface ILocationTypeControllerService
    {
        Task<Option<LocationTypeDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}