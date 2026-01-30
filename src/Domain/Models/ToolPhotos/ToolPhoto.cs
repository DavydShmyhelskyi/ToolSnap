using Domain.Models.Tools;

namespace Domain.Models.ToolPhotos
{
    public class ToolPhoto
    {
        public ToolPhotoId Id { get; }
        public string OriginalName { get; }
        public ToolId ToolId { get; }
        public PhotoTypeId PhotoTypeId { get; }
        public DateTimeOffset UploadDate { get; }

        public Tool? Tool { get; private set; }

        private ToolPhoto(
            ToolPhotoId id,
            string originalName,
            ToolId toolId,
            PhotoTypeId photoTypeId)
        {
            Id = id;
            OriginalName = originalName;
            ToolId = toolId;
            PhotoTypeId = photoTypeId;
            UploadDate = DateTimeOffset.UtcNow;
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
                photoTypeId
            );
        }

        public string GetFilePath()
            => $"{ToolId}/{Id}{Path.GetExtension(OriginalName)}";
    }
}
