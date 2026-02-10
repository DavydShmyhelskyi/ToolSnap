using Application.Entities.ToolAssignments.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ToolAssignmentErrorFactory
{
    public static ObjectResult ToObjectResult(this ToolAssignmentException error)
        => new(error.Message)
        {
            StatusCode = error switch
            {
                ToolAssignmentAlreadyExistsException => StatusCodes.Status409Conflict,
                ToolAssignmentNotFoundException => StatusCodes.Status404NotFound,
                ToolAssignmentAlreadyReturnedException => StatusCodes.Status409Conflict,
                DetectedToolNotFoundForToolAssignmentException => StatusCodes.Status404NotFound,
                ToolNotFoundForToolAssignmentException => StatusCodes.Status404NotFound,
                UserNotFoundForToolAssignmentException => StatusCodes.Status404NotFound,
                LocationNotFoundForToolAssignmentException => StatusCodes.Status404NotFound,
                UnhandledToolAssignmentException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Tool assignment error handler not implemented")
            }
        };
}