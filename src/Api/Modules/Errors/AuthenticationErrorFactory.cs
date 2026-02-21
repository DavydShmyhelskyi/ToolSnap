using Application.Entities.Authentication.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class AuthenticationErrorFactory
{
    public static ObjectResult ToObjectResult(this AuthenticationException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                InvalidCredentialsException => StatusCodes.Status401Unauthorized,
                EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                UserNotActiveException => StatusCodes.Status403Forbidden,
                RoleNotFoundForAuthenticationException => StatusCodes.Status404NotFound,
                DefaultRoleNotFoundException => StatusCodes.Status500InternalServerError,
                InvalidRefreshTokenException => StatusCodes.Status401Unauthorized,
                RefreshTokenExpiredException => StatusCodes.Status401Unauthorized,
                UnhandledAuthenticationException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Authentication error handler not implemented")
            }
        };
}