using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.ToolPhotos;

namespace Domain.Models.Tools
{
    public class Tool
    {
        public ToolId Id { get; }
        public ToolTypeId ToolTypeId { get; private set; }
        public BrandId? BrandId { get; private set; }
        public ModelId? ModelId { get; private set; }
        public string? SerialNumber { get; private set; }
        public ToolStatusId ToolStatusId { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        // navigation properties
        public ToolStatus? ToolStatus { get; private set; }

        public IReadOnlyCollection<ToolPhoto> Photos => _photos;
        private readonly List<ToolPhoto> _photos = new();

        public IReadOnlyCollection<ToolAssignment> Assignments => _assignments;
        private readonly List<ToolAssignment> _assignments = new();
        private Tool(
            ToolId id,
            ToolTypeId toolTypeId,
            BrandId? brandId,
            ModelId? modelId,
            string? serialNumber,
            ToolStatusId toolStatusId,
            DateTimeOffset createdAt)
        {
            Id = id;
            ToolTypeId = toolTypeId;
            BrandId = brandId;
            ModelId = modelId;
            SerialNumber = serialNumber;
            ToolStatusId = toolStatusId;
            CreatedAt = createdAt;
        }

        public static Tool New(
            ToolTypeId toolTypeId,
            ToolStatusId toolStatusId,
            BrandId? brandId,
            ModelId? modelId,
            string? serialNumber)
        {
            return new Tool(
                ToolId.New(),
                toolTypeId,
                brandId,
                modelId,
                serialNumber,
                toolStatusId,
                DateTimeOffset.UtcNow
            );
        }

        public void ChangeStatus(ToolStatusId toolStatusId)
        {
            ToolStatusId = toolStatusId;
        }
    }
}
