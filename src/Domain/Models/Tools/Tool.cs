using Domain.Models.ToolAssignments;
using Domain.Models.ToolInfo;
using Domain.Models.ToolPhotos;

namespace Domain.Models.Tools
{
    public class Tool
    {
        public ToolId Id { get; }
        public ToolTypeId ToolTypeId { get; init; }
        public BrandId? BrandId { get; private set; }
        public ModelId? ModelId { get; private set; }
        public string? SerialNumber { get; private set; }
        public ToolStatusId ToolStatusId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedAt { get; }

        // navigation properties
        public ToolStatus? ToolStatus { get; private set; }
        public IEnumerable<ToolPhoto> ToolPhotos { get; private set; } = new List<ToolPhoto>();
        public ToolType? ToolType { get; private set; }
        public Brand? Brand { get; private set; }
        public Model? Model { get; private set; }
        public IEnumerable<ToolAssignment> ToolAssignments { get; private set; } = new List<ToolAssignment>();

        private Tool(
            ToolId id,
            ToolTypeId toolTypeId,
            BrandId? brandId,
            ModelId? modelId,
            string? serialNumber,
            ToolStatusId toolStatusId,
            decimal price,
            DateTime createdAt)
        {
            Id = id;
            ToolTypeId = toolTypeId;
            BrandId = brandId;
            ModelId = modelId;
            SerialNumber = serialNumber;
            ToolStatusId = toolStatusId;
            Price = price;
            CreatedAt = createdAt;
        }

        public static Tool New(
            ToolTypeId toolTypeId,
            BrandId? brandId,
            ModelId? modelId,
            ToolStatusId toolStatusId,
            string? serialNumber = null,
            decimal price = 0)
        {
            return new Tool(
                ToolId.New(),
                toolTypeId,
                brandId,
                modelId,
                serialNumber,
                toolStatusId,
                price,
                DateTime.UtcNow);
        }

        public void Update(
            BrandId? brandId,
            ModelId? modelId,
            string? serialNumber,
            decimal price)
        {
            BrandId = brandId;
            ModelId = modelId;
            SerialNumber = serialNumber;
            Price = price;
        }

        public void ChangeStatus(ToolStatusId toolStatusId)
        {
            ToolStatusId = toolStatusId;
        }
    }
}
