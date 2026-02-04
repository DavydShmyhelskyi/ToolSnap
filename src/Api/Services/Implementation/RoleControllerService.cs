using Domain.Models.Roles;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;


namespace Api.Services.Implementation
{
    public class RoleControllerService(IRolesQueries roleQueries) : IRoleControllerService
    {
        public async Task<Option<RoleDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await roleQueries.GetByIdAsync(new RoleId(id), cancellationToken);

            return entity.Match(
                r => RoleDto.FromDomain(r),
                () => Option<RoleDto>.None);
        }
    }
}
