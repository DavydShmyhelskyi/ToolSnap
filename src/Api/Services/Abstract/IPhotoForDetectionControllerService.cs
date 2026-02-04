using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IPhotoForDetectionControllerService
    {
        Task<Option<PhotoForDetectionDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}