using Domain.Models.ToolPhotos;

namespace Api.DTOs
{
    public record ToolPhotoDto(
        Guid Id,
        string OriginalName,
        Guid ToolId,
        DateTime UploadDate)
    {
        public static ToolPhotoDto FromDomain(ToolPhoto toolPhoto) =>
            new(
                toolPhoto.Id.Value,
                toolPhoto.OriginalName,
                toolPhoto.ToolId.Value,
                toolPhoto.UploadDate);
    }

    public class CreateToolPhotoDto
    {
        public Guid ToolId { get; set; }
        public Guid PhotoTypeId { get; set; }
        public IFormFile File { get; set; } = default!;
    }

    public record ToolPhotoFileDto(
        string FileName,
        byte[] Content 
    );
}