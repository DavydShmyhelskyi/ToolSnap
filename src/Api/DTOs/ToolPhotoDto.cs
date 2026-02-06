using Domain.Models.ToolPhotos;

namespace Api.DTOs
{
    public record ToolPhotoDto(
        Guid Id,
        string OriginalName,
        Guid ToolId,
        Guid PhotoTypeId)
    {
        public static ToolPhotoDto FromDomain(ToolPhoto toolPhoto) =>
            new(
                toolPhoto.Id.Value,
                toolPhoto.OriginalName,
                toolPhoto.ToolId.Value,
                toolPhoto.PhotoTypeId.Value);
    }

    public record CreateToolPhotoDto(
        Guid ToolId,
        Guid PhotoTypeId,
        string OriginalName);
}