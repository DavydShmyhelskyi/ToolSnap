using Domain.Models.PhotoSessions;

namespace Api.DTOs
{
    public record PhotoForDetectionDto(
        Guid Id,
        Guid PhotoSessionId,
        string OriginalName,
        DateTimeOffset UploadDate)
    {
        public static PhotoForDetectionDto FromDomain(PhotoForDetection photoForDetection) =>
            new(
                photoForDetection.Id.Value,
                photoForDetection.PhotoSessionId.Value,
                photoForDetection.OriginalName,
                photoForDetection.UploadDate);
    }

    public record CreatePhotoForDetectionDto(
    Guid PhotoSessionId,
    IFormFile File);
}