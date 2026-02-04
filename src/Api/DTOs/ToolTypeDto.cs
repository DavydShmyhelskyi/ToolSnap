using Domain.Models.ToolInfo;

namespace Api.DTOs
{
    public record ToolTypeDto(
        Guid Id,
        string Title)
    {
        public static ToolTypeDto FromDomain(ToolType toolType) =>
            new(
                toolType.Id.Value,
                toolType.Title);
    }

    public record CreateToolTypeDto(
        string Title);

    public record UpdateToolTypeDto(
        string Title);
}