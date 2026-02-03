using Domain.Models.ToolPhotos;

namespace Domain.Models.PhotoSessions
{
    public class PhotoForDetection
    {
        public PhotoForDetectionId Id { get; }
        public PhotoSessionId PhotoSessionId { get; }
        public string OriginalName { get; }
        public DateTimeOffset UploadDate { get; }

        // navigation properties
        public PhotoSession? Tool { get; private set; }

        private PhotoForDetection(
            PhotoForDetectionId id,
            PhotoSessionId photoSessionId,
            string originalName)
        {
            Id = id;
            PhotoSessionId = photoSessionId;
            OriginalName = originalName;
            UploadDate = DateTimeOffset.UtcNow;
        }

        public static PhotoForDetection New(
            PhotoForDetectionId id,
            PhotoSessionId photoSessionId,
            string originalName)
        {
            return new PhotoForDetection(
                PhotoForDetectionId.New(),
                photoSessionId,
                originalName
            );
        }

        public string GetFilePath()
            => $"{PhotoSessionId}/{Id}{Path.GetExtension(OriginalName)}";
    }
}
