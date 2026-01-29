namespace Domain.LogModels.PhotoSessions
{
    public sealed record PhotoSessionId(Guid Value)
    {
        public static PhotoSessionId Empty() => new(Guid.Empty);
        public static PhotoSessionId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
