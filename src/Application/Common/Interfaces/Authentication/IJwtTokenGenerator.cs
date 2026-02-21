using Domain.Models.Users;

namespace Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, string roleName);
    string GenerateRefreshToken();
}