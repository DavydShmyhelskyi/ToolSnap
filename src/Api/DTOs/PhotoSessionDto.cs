using Domain.Models.PhotoSessions;

namespace Api.DTOs
{
    public record PhotoSessionDto(
        Guid Id,
        double Latitude,
        double Longitude,
        Guid ActionTypeId,
        DateTimeOffset CreatedAt)
    {
        public static PhotoSessionDto FromDomain(PhotoSession photoSession) =>
            new(
                photoSession.Id.Value,
                photoSession.Latitude,
                photoSession.Longitude,
                photoSession.ActionTypeId.Value,
                photoSession.CreatedAt);
    }

    public record CreatePhotoSessionDto(
        double Latitude,
        double Longitude,
        Guid ActionTypeId);
}