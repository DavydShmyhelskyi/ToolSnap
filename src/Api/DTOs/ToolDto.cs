using Domain.Models.Tools;

namespace Api.DTOs
{
    public record ToolDto(
        Guid Id,
        string Name,
        string? Brand,
        string? Model,
        string? SerialNumber,
        Guid ToolStatusId,
        string? ToolStatusTitle,
        DateTimeOffset CreatedAt)
    {
        public static ToolDto FromDomain(Tool tool) =>
            new(
                tool.Id.Value,
                tool.Name,
                tool.Brand,
                tool.Model,
                tool.SerialNumber,
                tool.ToolStatusId.Value,
                tool.ToolStatus?.Title,
                tool.CreatedAt);
    }

    public record CreateToolDto(
        string Name,
        Guid ToolStatusId,
        string? Brand,
        string? Model,
        string? SerialNumber);

    public record UpdateToolDto(Guid ToolStatusId);
}