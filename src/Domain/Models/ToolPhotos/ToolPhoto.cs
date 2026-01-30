using Domain.Models.Tools;

namespace Domain.Models.ToolPhotos
{
    public class ToolPhoto
    {
        public ToolPhotoId Id { get; }
        public ToolId ToolId { get; private set; }
        public string PhotoUrl { get; private set; }
        public PhotoTypeId PhotoTypeId { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        private ToolPhoto(ToolPhotoId id, ToolId toolId, string photoUrl, PhotoTypeId photoTypeId, DateTimeOffset createdAt)
        {
            Id = id;
            ToolId = toolId;
            PhotoUrl = photoUrl;
            PhotoTypeId = photoTypeId;
            CreatedAt = createdAt;
        }

        public static ToolPhoto New(ToolId toolId, string photoUrl, PhotoTypeId photoTypeId)
        {
            return new ToolPhoto(ToolPhotoId.New(), toolId, photoUrl.Trim(), photoTypeId, DateTimeOffset.UtcNow);
        }

        public void UpdatePhotoUrl(string photoUrl)
        {
            PhotoUrl = photoUrl.Trim();
        }
    }
}