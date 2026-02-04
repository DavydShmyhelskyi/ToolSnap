using Domain.Models.ToolPhotos;

namespace Domain.Models.PhotoSessions
{
    public class PhotoForDetection
    {
        public PhotoForDetectionId Id { get; }
        public PhotoSessionId PhotoSessionId { get; }
        public string OriginalName { get; }
        public DateTime UploadDate { get; }

        // navigation properties
        public PhotoSession? PhotoSession { get; }

        private PhotoForDetection(
            PhotoForDetectionId id,
            PhotoSessionId photoSessionId,
            string originalName,
            DateTime uploadDate)
        {
            Id = id;
            PhotoSessionId = photoSessionId;
            OriginalName = originalName;
            UploadDate = uploadDate;
        }

        public static PhotoForDetection New(
            PhotoForDetectionId id,
            PhotoSessionId photoSessionId,
            string originalName)
        {
            return new PhotoForDetection(
                PhotoForDetectionId.New(),
                photoSessionId,
                originalName,
                DateTime.UtcNow
            );
        }

        public string GetFilePath()
            => $"{PhotoSessionId}/{Id}{Path.GetExtension(OriginalName)}";
    }
}
