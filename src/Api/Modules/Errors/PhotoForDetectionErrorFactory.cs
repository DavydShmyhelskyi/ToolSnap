using Application.Entities.PhotoForDetections.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PhotoForDetectionErrorFactory
{
    public static ObjectResult ToObjectResult(this PhotoForDetectionException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                PhotoForDetectionNotFoundException => StatusCodes.Status404NotFound,
                UnhandledPhotoForDetectionException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("PhotoForDetection error handler not implemented")
            }
        };
}