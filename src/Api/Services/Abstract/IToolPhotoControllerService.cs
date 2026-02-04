using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IToolPhotoControllerService
    {
        Task<Option<ToolPhotoDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}