using Application.Entities.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class UserErrorFactory
{
    public static ObjectResult ToObjectResult(this UserException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                UserAlreadyExistsException => StatusCodes.Status409Conflict,
                UserNotFoundException => StatusCodes.Status404NotFound,
                RoleNotFoundForUserException => StatusCodes.Status404NotFound,
                UnhandledUserException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("User error handler not implemented")
            }
        };
}
