using Domain.Models.ToolInfo;

namespace Api.DTOs
{
    public record BrandDto(
        Guid Id,
        string Title)
    {
        public static BrandDto FromDomain(Brand brand) =>
            new(
                brand.Id.Value,
                brand.Title);
    }

    public record CreateBrandDto(
        string Title);

    public record UpdateBrandDto(
        string Title);
}