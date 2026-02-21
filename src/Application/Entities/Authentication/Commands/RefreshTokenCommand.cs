using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Application.Entities.Authentication.Exceptions;
using LanguageExt;
using MediatR;

namespace Application.Entities.Authentication.Commands
{
    public record RefreshTokenCommand : IRequest<Either<AuthenticationException, AuthenticationResult>>
    {
        public required string RefreshToken { get; init; }
    }

    public class RefreshTokenCommandHandler(
        IUsersQueries usersQueries,
        IUsersRepository usersRepository,
        IRolesQueries rolesQueries,
        IJwtTokenGenerator jwtTokenGenerator,
        JwtSettings jwtSettings)
        : IRequestHandler<RefreshTokenCommand, Either<AuthenticationException, AuthenticationResult>>
    {
        public async Task<Either<AuthenticationException, AuthenticationResult>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            // Find user by refresh token (with tracking, without includes)
            var user = await usersQueries.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (user == null)
                return new InvalidRefreshTokenException();

            // Validate refresh token
            if (!user.IsRefreshTokenValid(request.RefreshToken))
                return new RefreshTokenExpiredException();

            // Check if user is active
            if (!user.IsActive)
                return new UserNotActiveException(user.Id);

            // Get role name
            var roleOption = await rolesQueries.GetByIdAsync(user.RoleId, cancellationToken);

            if (roleOption.IsNone)
                return new RoleNotFoundForAuthenticationException(user.RoleId);

            var roleName = roleOption.Match(r => r.Title, () => "User");

            // Generate new tokens
            var newAccessToken = jwtTokenGenerator.GenerateAccessToken(user, roleName);
            var newRefreshToken = jwtTokenGenerator.GenerateRefreshToken();

            // Update refresh token (token rotation)
            user.SetRefreshToken(
                newRefreshToken,
                DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays));

            // Save changes (user already tracked)
            await usersRepository.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult(user, newAccessToken, newRefreshToken);
        }
    }
}