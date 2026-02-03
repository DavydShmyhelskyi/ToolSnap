namespace Domain.Models.DetectedTools
{
    public record DetectedToolId(Guid Value)
    {
        public static DetectedToolId Empty() => new(Guid.Empty);
        public static DetectedToolId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
