using Domain.Models.PhotoSessions;
using Domain.Models.DetectedTools;
using Domain.Models.ToolInfo;

namespace Application.Entities.DetectedTools.Exceptions
{
    public abstract class DetectedToolException(DetectedToolId id, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public DetectedToolId DetectedToolId { get; } = id;
    }

    public class DetectedToolNotFoundException(DetectedToolId id)
        : DetectedToolException(id, $"Detected tool with id '{id}' was not found.");

    public class PhotoSessionNotFoundForDetectedToolException(PhotoSessionId photoSessionId)
        : DetectedToolException(DetectedToolId.Empty(), $"Photo session with id '{photoSessionId}' was not found. Cannot create detected tool.")
    {
        public PhotoSessionId PhotoSessionId { get; } = photoSessionId;
    }

    public class ToolTypeNotFoundForDetectedToolException(ToolTypeId toolTypeId)
        : DetectedToolException(DetectedToolId.Empty(), $"Tool type with id '{toolTypeId}' was not found. Cannot create detected tool.")
    {
        public ToolTypeId ToolTypeId { get; } = toolTypeId;
    }

    public class BrandNotFoundForDetectedToolException(BrandId brandId)
        : DetectedToolException(DetectedToolId.Empty(), $"Brand with id '{brandId}' was not found. Cannot create detected tool.")
    {
        public BrandId BrandId { get; } = brandId;
    }

    public class ModelNotFoundForDetectedToolException(ModelId modelId)
        : DetectedToolException(DetectedToolId.Empty(), $"Model with id '{modelId}' was not found. Cannot create detected tool.")
    {
        public ModelId ModelId { get; } = modelId;
    }

    public class UnhandledDetectedToolException(DetectedToolId id, Exception? innerException = null)
        : DetectedToolException(id, "Unexpected error occured", innerException);
}