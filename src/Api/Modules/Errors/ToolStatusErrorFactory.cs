using Application.Entities.ToolStatuses.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolStatusErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolStatusException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolStatusAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolStatusNotFoundException => StatusCodes.Status404NotFound,
                UnhandledToolStatusException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool status error handler not implemented")
            }
        };
}
