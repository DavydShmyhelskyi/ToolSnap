using Domain.Models.ToolInfo;

namespace Api.DTOs
{
    public record ToolStatusDto(
        Guid Id,
        string Title)
    {
        public static ToolStatusDto FromDomain(ToolStatus toolStatus) =>
            new(
                toolStatus.Id.Value,
                toolStatus.Title);
    }

    public record CreateToolStatusDto(
        string Title);

    public record UpdateToolStatusDto(
        string Title);
}