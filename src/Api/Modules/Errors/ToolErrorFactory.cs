using Application.Entities.Tools.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolNotFoundException => StatusCodes.Status404NotFound,
                ToolTypeNotFoundForToolException => StatusCodes.Status404NotFound,
                BrandNotFoundForToolException => StatusCodes.Status404NotFound,
                ModelNotFoundForToolException => StatusCodes.Status404NotFound,
                ToolStatusNotFoundForToolException => StatusCodes.Status404NotFound,
                UnhandledToolException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool error handler not implemented")
            }
        };
}
