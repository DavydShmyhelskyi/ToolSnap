using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IDetectedToolControllerService
    {
        Task<Option<DetectedToolDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}