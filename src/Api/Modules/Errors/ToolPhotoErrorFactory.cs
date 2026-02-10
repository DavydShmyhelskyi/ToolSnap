using Application.Entities.ToolPhotos.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolPhotoErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolPhotoException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolPhotoAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolPhotoNotFoundException => StatusCodes.Status404NotFound,
                ToolNotFoundForToolPhotoException => StatusCodes.Status404NotFound,
                UnhandledToolPhotoException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool photo error handler not implemented")
            }
        };
}
