using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IPhotoTypeControllerService
    {
        Task<Option<PhotoTypeDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}