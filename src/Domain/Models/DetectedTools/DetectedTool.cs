using Domain.Models.PhotoSessions;
using Domain.Models.ToolInfo;

namespace Domain.Models.DetectedTools
{
    public class DetectedTool
    {
        public DetectedToolId Id { get; init; }
        public PhotoSessionId PhotoSessionId { get; init; }
        public ToolTypeId ToolTypeId { get; init; }
        public BrandId? BrandId { get; private set; }
        public ModelId? ModelId { get; private set; }
        public string? SerialNumber { get; init; }
        public float Confidence { get; init; }
        public bool RedFlagged { get; init; }

        private DetectedTool(
            DetectedToolId id,
            PhotoSessionId photoSessionId,
            ToolTypeId toolTypeId,
            BrandId? brandId,
            ModelId? modelId,
            string? serialNumber,
            float confidence,
            bool redFlagged)
        {
            Id = id;
            PhotoSessionId = photoSessionId;
            ToolTypeId = toolTypeId;
            BrandId = brandId;
            ModelId = modelId;
            SerialNumber = serialNumber;
            Confidence = confidence;
            RedFlagged = redFlagged;
        }

        public static DetectedTool New(
            ToolTypeId toolTypeId,
         PhotoSessionId photoSessionId,
         BrandId? brandId,
         ModelId? modelId,
         string? serialNumber,
         float confidence,
         bool redFlagged)
        {
            return new DetectedTool(
                DetectedToolId.New(),
                photoSessionId,
                toolTypeId,
                brandId,
                modelId,
                serialNumber,
                confidence,
                redFlagged
            );
        }
    }
}