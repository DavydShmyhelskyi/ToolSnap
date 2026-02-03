using Domain.Models.ToolPhotos;

namespace Api.DTOs
{
    public record ToolPhotoDto(
        Guid Id,
        string OriginalName,
        Guid ToolId,
        DateTimeOffset UploadDate,
        string FilePath)
    {
        public static ToolPhotoDto FromDomain(ToolPhoto toolPhoto) =>
            new(
                toolPhoto.Id.Value,
                toolPhoto.OriginalName,
                toolPhoto.ToolId.Value,
                toolPhoto.UploadDate,
                toolPhoto.GetFilePath());
    }

    public record CreateToolPhotoDto(
        Guid ToolId,
        Guid PhotoTypeId,
        string OriginalName);

    public record UpdateToolPhotoDto(Guid PhotoTypeId);
}