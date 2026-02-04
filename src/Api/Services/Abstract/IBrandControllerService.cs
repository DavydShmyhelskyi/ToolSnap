using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IBrandControllerService
    {
        Task<Option<BrandDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}