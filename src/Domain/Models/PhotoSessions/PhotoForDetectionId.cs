namespace Domain.Models.PhotoSessions
{
    public record PhotoForDetectionId(Guid Value)
    {
        public static PhotoForDetectionId Empty() => new(Guid.Empty);
        public static PhotoForDetectionId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
