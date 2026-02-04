using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IPhotoSessionControllerService
    {
        Task<Option<PhotoSessionDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}