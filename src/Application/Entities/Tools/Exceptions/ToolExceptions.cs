using Domain.Models.Tools;
using Domain.Models.ToolInfo;

namespace Application.Entities.Tools.Exceptions
{
    public abstract class ToolException(ToolId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public ToolId ToolId { get; } = id;
    }

    public class ToolNotFoundException(ToolId id)
        : ToolException(id, $"Tool with id '{id}' was not found.");

    public class ToolAlreadyExistsException(ToolId id)
        : ToolException(id, $"Tool with id '{id}' already exists.");

    public class UnhandledToolException(ToolId id, Exception? innerException = null)
        : ToolException(id, "Unexpected error occured", innerException);

    public class ToolStatusNotFoundForToolException(ToolStatusId toolStatusId)
        : ToolException(ToolId.Empty(), $"Tool status with id '{toolStatusId}' was not found.");

    public class ToolTypeNotFoundForToolException(ToolTypeId toolTypeId)
        : ToolException(ToolId.Empty(), $"Tool type with id '{toolTypeId}' was not found.");

    public class BrandNotFoundForToolException(BrandId brandId)
        : ToolException(ToolId.Empty(), $"Brand with id '{brandId}' was not found.");

    public class ModelNotFoundForToolException(ModelId modelId)
        : ToolException(ToolId.Empty(), $"Model with id '{modelId}' was not found.");
}