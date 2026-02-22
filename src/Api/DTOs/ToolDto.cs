using Domain.Models.Tools;

namespace Api.DTOs
{
    public record ToolDto(
        Guid Id,
        Guid ToolTypeId,
        Guid? BrandId,
        Guid? ModelId,
        string? SerialNumber,
        Guid ToolStatusId,
        DateTimeOffset CreatedAt)
    {
        public static ToolDto FromDomain(Tool tool) =>
            new(
                tool.Id.Value,
                tool.ToolTypeId.Value,
                tool.BrandId?.Value,
                tool.ModelId?.Value,
                tool.SerialNumber,
                tool.ToolStatusId.Value,
                tool.CreatedAt);
    }

    public record CreateToolDto(
        Guid ToolTypeId,
        Guid? BrandId,
        Guid? ModelId,
        Guid ToolStatusId,
        string? SerialNumber);

    public record UpdateToolDto(
        Guid? BrandId,
        Guid? ModelId,
        string? SerialNumber);

    public record ChangeToolStatusDto(
        Guid ToolStatusId);
    public class CreateToolWithAssignmentDto
    {
        // Основні дані
        public Guid UserId { get; set; }
        public Guid ActionTypeId { get; set; }
        public Guid PhotoTypeId { get; set; }
        public Guid LocationId { get; set; }

        public Guid ToolTypeId { get; set; }
        public Guid ToolStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ModelId { get; set; }
        public string? SerialNumber { get; set; }

        // Фото
        public IFormFile File { get; set; } = default!;
    }
}