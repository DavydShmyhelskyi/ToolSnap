using Domain.Models.ToolInfo;
using Domain.Models.Tools;

namespace Api.DTOs
{
    public record ToolDto(
        Guid Id,
        Guid ToolType,
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
        string Name,
        Guid ToolStatusId,
        string? Brand,
        string? Model,
        string? SerialNumber);

    public record UpdateToolDto(Guid ToolStatusId);
}