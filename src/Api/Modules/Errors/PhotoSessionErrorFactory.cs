using Application.Entities.PhotoSessions.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PhotoSessionErrorFactory
{
    public static ObjectResult ToObjectResult(this PhotoSessionException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                PhotoSessionNotFoundException => StatusCodes.Status404NotFound,
                UnhandledPhotoSessionException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Photo session error handler not implemented")
            }
        };
}