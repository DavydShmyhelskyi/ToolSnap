using Application.Entities.ToolTypes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolTypeErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolTypeException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolTypeNotFoundException => StatusCodes.Status404NotFound,
                UnhandledToolTypeException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool type error handler not implemented")
            }
        };
}