using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Queries;
using Application.Entities.Authentication.Exceptions;
using Domain.Models.Users;
using LanguageExt;
using MediatR;

namespace Application.Entities.Authentication.Commands
{
    public record LoginCommand : IRequest<Either<AuthenticationException, AuthenticationResult>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }

    public record AuthenticationResult(
        User User,
        string Token);

    public class LoginCommandHandler(
        IUsersQueries usersQueries,
        IRolesQueries rolesQueries,
        IJwtTokenGenerator jwtTokenGenerator)
        : IRequestHandler<LoginCommand, Either<AuthenticationException, AuthenticationResult>>
    {
        public async Task<Either<AuthenticationException, AuthenticationResult>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            // Find user by email
            var userOption = await usersQueries.GetByEmailAsync(request.Email, cancellationToken);

            if (userOption.IsNone)
                return new InvalidCredentialsException();

            var user = userOption.Match(u => u, () => throw new InvalidOperationException());

            // Verify password
            if (!user.VerifyPassword(request.Password))
                return new InvalidCredentialsException();

            // Check if user is active
            if (!user.IsActive)
                return new UserNotActiveException(user.Id);

            // Get role name
            var roleOption = await rolesQueries.GetByIdAsync(user.RoleId, cancellationToken);
            
            if (roleOption.IsNone)
                return new RoleNotFoundForAuthenticationException(user.RoleId);

            var roleName = roleOption.Match(r => r.Title, () => "User");

            // Generate JWT token
            var token = jwtTokenGenerator.GenerateToken(user, roleName);

            return new AuthenticationResult(user, token);
        }
    }
}