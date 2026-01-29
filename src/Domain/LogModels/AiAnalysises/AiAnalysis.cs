using Domain.LogModels.PhotoSessions;
using Domain.LogModels.ValueObjects;

namespace Domain.LogModels.AiAnalysises
{
    public class AiAnalysis
    {
        public AiAnalysisId Id { get; private set; }
        public PhotoSessionId PhotoSessionId { get; private set; }
        public IReadOnlyList<DetectedTool> DetectedTools { get; private set; }
        public Confidence Confidence { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private AiAnalysis() { }

        public static AiAnalysis New(PhotoSessionId photoSessionId, IEnumerable<DetectedTool> detectedTools, Confidence confidence)
        {
            if (photoSessionId is null) throw new ArgumentNullException(nameof(photoSessionId));
            if (detectedTools is null) throw new ArgumentNullException(nameof(detectedTools));
            if (confidence is null) throw new ArgumentNullException(nameof(confidence));

            var list = new List<DetectedTool>(detectedTools);

            return new AiAnalysis
            {
                Id = AiAnalysisId.New(),
                PhotoSessionId = photoSessionId,
                DetectedTools = list.AsReadOnly(),
                Confidence = confidence,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
