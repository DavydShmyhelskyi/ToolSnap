using Domain.Models.Tools;

namespace Domain.Models.ToolPhotos
{
    public class ToolPhoto
    {
        public ToolPhotoId Id { get; }
        public string OriginalName { get; }
        public ToolId ToolId { get; }
        public DateTime UploadDate { get; }
        public PhotoTypeId PhotoTypeId { get; }

        // navigation properties
        public PhotoType? PhotoType { get; private set; }
        public Tool? Tool { get; private set; }

        private ToolPhoto(
            ToolPhotoId id,
            string originalName,
            ToolId toolId,
            PhotoTypeId photoTypeId,
            DateTime uploadDate)
            
        {
            Id = id;
            OriginalName = originalName;
            ToolId = toolId;
            PhotoTypeId = photoTypeId;
            UploadDate = uploadDate;
        }

        public static ToolPhoto New(
            ToolId toolId,
            PhotoTypeId photoTypeId,
            string originalName)
        {
            if (string.IsNullOrWhiteSpace(originalName))
                throw new ArgumentException("OriginalName is required.", nameof(originalName));

            return new ToolPhoto(
                ToolPhotoId.New(),
                originalName,
                toolId,
                photoTypeId,
                DateTime.UtcNow
            );
        }

        public string GetFilePath()
            => $"{ToolId}/{Id}{Path.GetExtension(OriginalName)}";
    }
}
