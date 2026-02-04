using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface ILocationControllerService
    {
        Task<Option<LocationDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}