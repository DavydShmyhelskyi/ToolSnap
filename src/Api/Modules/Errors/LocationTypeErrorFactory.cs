using Application.Entities.LocationTypes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class LocationTypeErrorFactory
{
    public static ObjectResult ToObjectResult(this LocationTypeException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                LocationTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                LocationTypeNotFoundException => StatusCodes.Status404NotFound,
                UnhandledLocationTypeException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("LocationType error handler not implemented")
            }
        };
}