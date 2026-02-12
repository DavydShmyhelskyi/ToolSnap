using Application.Entities.PhotoTypes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PhotoTypeErrorFactory
{
    public static ObjectResult ToObjectResult(this PhotoTypeException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                PhotoTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                PhotoTypeNotFoundException => StatusCodes.Status404NotFound,
                UnhandledPhotoTypeException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("PhotoType error handler not implemented")
            }
        };
}
