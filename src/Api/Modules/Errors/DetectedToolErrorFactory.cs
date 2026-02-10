using Application.Entities.DetectedTools.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class DetectedToolErrorFactory
{
    public static ObjectResult ToObjectResult(this DetectedToolException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                DetectedToolNotFoundException => StatusCodes.Status404NotFound,
                UnhandledDetectedToolException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("DetectedTool error handler not implemented")
            }
        };
}