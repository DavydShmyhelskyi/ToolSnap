using Application.Entities.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ModelErrorFactory
{
    public static ObjectResult ToObjectResult(this ModelException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ModelAlreadyExistsException => StatusCodes.Status409Conflict,
                ModelNotFoundException => StatusCodes.Status404NotFound,
                UnhandledModelException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Model error handler not implemented")
            }
        };
}