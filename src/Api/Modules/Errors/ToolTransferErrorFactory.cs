using Application.Entities.ToolTransfers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolTransferErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolTransferException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolTransferNotFoundException => StatusCodes.Status404NotFound,
                ToolTransferNotPendingException => StatusCodes.Status409Conflict,
                ToolTransferUnauthorizedException => StatusCodes.Status403Forbidden,
                ToolTransferSelfTransferException => StatusCodes.Status400BadRequest,
                ToolTransferPendingAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolNotAssignedToUserException => StatusCodes.Status409Conflict,
                UserNotFoundForToolTransferException => StatusCodes.Status404NotFound,
                ToolNotFoundForToolTransferException => StatusCodes.Status404NotFound,
                ToolAssignmentNotFoundForToolTransferException => StatusCodes.Status404NotFound,
                UnhandledToolTransferException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool transfer error handler not implemented")
            }
        };
}
