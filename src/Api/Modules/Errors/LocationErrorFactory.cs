using Application.Entities.Locations.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class LocationErrorFactory
{
    public static ObjectResult ToObjectResult(this LocationException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                LocationAlreadyExistsException => StatusCodes.Status409Conflict,
                LocationNotFoundException => StatusCodes.Status404NotFound,
                UnhandledLocationException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Location error handler not implemented")
            }
        };
}
