using Domain.Enums;
using Domain.Models.Tools;

namespace Domain.Models.ToolPhotos
{
    public class ToolPhoto
    {
        public ToolPhotoId Id { get; }
        public ToolId ToolId { get; private set; }
        public string PhotoUrl { get; private set; }
        public PhotoType PhotoType { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private ToolPhoto(ToolPhotoId id, ToolId toolId, string photoUrl, PhotoType photoType, DateTimeOffset createdAt)
        {
            Id = id;
            ToolId = toolId;
            PhotoUrl = photoUrl;
            PhotoType = photoType;
            CreatedAt = createdAt;
        }

        public static ToolPhoto New(ToolId toolId, string photoUrl, PhotoType photoType)
        {
            return new ToolPhoto(ToolPhotoId.New(), toolId, photoUrl.Trim(), photoType, DateTimeOffset.UtcNow);
        }

        public void UpdatePhotoUrl(string photoUrl)
        {
            PhotoUrl = photoUrl.Trim();
        }
    }
}