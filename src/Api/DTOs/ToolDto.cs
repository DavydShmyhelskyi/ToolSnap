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
}