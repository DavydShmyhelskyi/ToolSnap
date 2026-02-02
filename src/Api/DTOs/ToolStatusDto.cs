using Domain.Models.Tools;

namespace Api.DTOs
{
    public record ToolStatusDto(Guid Id, string Title)
    {
        public static ToolStatusDto FromDomain(ToolStatus toolStatus) =>
            new(
                toolStatus.Id.Value,
                toolStatus.Title);
    }

    public record CreateToolStatusDto(string Title);

    public record UpdateToolStatusDto(string Title);
}