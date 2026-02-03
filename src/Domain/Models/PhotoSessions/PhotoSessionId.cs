namespace Domain.Models.PhotoSessions
{
    public record PhotoSessionId(Guid Value)
    {
        public static PhotoSessionId Empty() => new(Guid.Empty);
        public static PhotoSessionId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
