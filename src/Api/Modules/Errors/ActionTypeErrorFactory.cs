using Application.Entities.ActionTypes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ActionTypeErrorFactory
{
    public static ObjectResult ToObjectResult(this ActionTypeException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ActionTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                ActionTypeNotFoundException => StatusCodes.Status404NotFound,
                UnhandledActionTypeException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Action type error handler not implemented")
            }
        };
}