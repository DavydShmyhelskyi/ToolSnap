using Domain.Models.DetectedTools;

namespace Api.DTOs
{
    public record DetectedToolDto(
        Guid Id,
        Guid PhotoSessionId,
        Guid ToolTypeId,
        Guid? BrandId,
        Guid? ModelId,
        string? SerialNumber,
        float Confidence,
        bool RedFlagged)
    {
        public static DetectedToolDto FromDomain(DetectedTool detectedTool) =>
            new(
                detectedTool.Id.Value,
                detectedTool.PhotoSessionId.Value,
                detectedTool.ToolTypeId.Value,
                detectedTool.BrandId?.Value,
                detectedTool.ModelId?.Value,
                detectedTool.SerialNumber,
                detectedTool.Confidence,
                detectedTool.RedFlagged);
    }

    public record CreateDetectedToolDto(
        Guid PhotoSessionId,
        Guid ToolTypeId,
        Guid? BrandId,
        Guid? ModelId,
        string? SerialNumber,
        float Confidence,
        bool RedFlagged);
}