using Api.DTOs;
using LanguageExt;

namespace Api.Services.Abstract
{
    public interface IRolesControllerService
    {
        Task<Option<RoleDto>> Get(Guid cityId, CancellationToken cancellationToken);
    }
}
