using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class RolesControllerService(IRolesQueries cityRepository) : IRolesControllerService
    {
        public async Task<Option<RoleDto>> Get(Guid cityId, CancellationToken cancellationToken)
        {
            var entity = await cityRepository.GetByIdAsync(new Domain.Models.Roles.RoleId(cityId), cancellationToken);
            return entity.Match(
                r => RoleDto.FromDomain(r),
                () => Option<RoleDto>.None);
        }
    }
}
