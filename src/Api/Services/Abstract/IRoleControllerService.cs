using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IRoleControllerService
    {
        Task<Option<RoleDto>> Get(Guid id, CancellationToken cancellationToken);
    }
}
