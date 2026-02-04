using Domain.Models.Users;
using Api.DTOs;
using Api.Services.Abstract;
using Application.Common.Interfaces.Queries;
using LanguageExt;

namespace Api.Services.Implementation
{
    public class UserControllerService(IUsersQueries usersQueries) : IUserControllerService
    {
        public async Task<Option<UserDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await usersQueries.GetByIdAsync(new UserId(id), cancellationToken);

            return entity.Match(
                u => UserDto.FromDomain(u),
                () => Option<UserDto>.None);
        }
    }
}