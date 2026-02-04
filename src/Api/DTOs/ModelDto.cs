using Domain.Models.ToolInfo;

namespace Api.DTOs
{
    public record ModelDto(
        Guid Id,
        string Title)
    {
        public static ModelDto FromDomain(Model model) =>
            new(
                model.Id.Value,
                model.Title);
    }

    public record CreateModelDto(
        string Title);

    public record UpdateModelDto(
        string Title);
}