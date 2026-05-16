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
        decimal Price,
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
                tool.Price,
                tool.CreatedAt);
    }

    public record CreateToolDto(
        Guid ToolTypeId,
        Guid? BrandId,
        Guid? ModelId,
        Guid ToolStatusId,
        string? SerialNumber,
        decimal Price = 0);

    public record UpdateToolDto(
        Guid? BrandId,
        Guid? ModelId,
        string? SerialNumber,
        decimal Price = 0);

    public record ChangeToolStatusDto(
        Guid ToolStatusId);

    public class CreateToolWithAssignmentDto
    {
        public Guid UserId { get; set; }
        public Guid ActionTypeId { get; set; }
        public Guid PhotoTypeId { get; set; }
        public Guid LocationId { get; set; }

        public Guid ToolTypeId { get; set; }
        public Guid ToolStatusId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ModelId { get; set; }
        public string? SerialNumber { get; set; }
        public decimal Price { get; set; } = 0;

        public IFormFile File { get; set; } = default!;
    }
}
