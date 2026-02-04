using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IUserControllerService
    {
        Task<Option<UserDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}