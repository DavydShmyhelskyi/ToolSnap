using Application.Entities.Brands.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class BrandErrorFactory
{
    public static ObjectResult ToObjectResult(this BrandException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                BrandAlreadyExistsException => StatusCodes.Status409Conflict,
                BrandNotFoundException => StatusCodes.Status404NotFound,
                UnhandledBrandException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Brand error handler not implemented")
            }
        };
}