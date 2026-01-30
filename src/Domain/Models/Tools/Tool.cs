using Domain.Models.ToolAssignments;
using Domain.Models.ToolPhotos;

namespace Domain.Models.Tools
{
    public class Tool
    {
        public ToolId Id { get; }
        public string Name { get; private set; }
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
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
            string name,
            string? brand,
            string? model,
            string? serialNumber,
            ToolStatusId toolStatusId,
            DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Model = model;
            SerialNumber = serialNumber;
            ToolStatusId = toolStatusId;
            CreatedAt = createdAt;
        }

        public static Tool New(
            string name,
            ToolStatusId toolStatusId,
            string? brand = null,
            string? model = null,
            string? serialNumber = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (toolStatusId == null || toolStatusId == ToolStatusId.Empty())
                throw new ArgumentException("ToolStatusId is required.", nameof(toolStatusId));

            return new Tool(
                ToolId.New(),
                name.Trim(),
                string.IsNullOrWhiteSpace(brand) ? null : brand.Trim(),
                string.IsNullOrWhiteSpace(model) ? null : model.Trim(),
                string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim(),
                toolStatusId,
                DateTimeOffset.UtcNow
            );
        }

        public void ChangeStatus(ToolStatusId toolStatusId)
        {
            if (toolStatusId == null || toolStatusId == ToolStatusId.Empty())
                throw new ArgumentException("ToolStatusId is required.", nameof(toolStatusId));

            ToolStatusId = toolStatusId;
        }
    }
}
