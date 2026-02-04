using Domain.Models.Tools;

namespace Domain.Models.ToolPhotos
{
    public class ToolPhoto
    {
        public ToolPhotoId Id { get; }
        public string OriginalName { get; }
        public ToolId ToolId { get; }
        public DateTime UploadDate { get; }

        // navigation properties

        private ToolPhoto(
            ToolPhotoId id,
            string originalName,
            ToolId toolId,
            DateTime uploadDate)
        {
            Id = id;
            OriginalName = originalName;
            ToolId = toolId;
            UploadDate = uploadDate;
        }

        public static ToolPhoto New(
            ToolId toolId,
            string originalName)
        {
            if (string.IsNullOrWhiteSpace(originalName))
                throw new ArgumentException("OriginalName is required.", nameof(originalName));

            return new ToolPhoto(
                ToolPhotoId.New(),
                originalName,
                toolId,
                DateTime.UtcNow
            );
        }

        public string GetFilePath()
            => $"{ToolId}/{Id}{Path.GetExtension(OriginalName)}";
    }
}
