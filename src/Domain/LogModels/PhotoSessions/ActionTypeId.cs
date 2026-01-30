namespace Domain.LogModels.PhotoSessions
{
    public record ActionTypeId(Guid Value)
    {
        public static ActionTypeId Empty() => new(Guid.Empty);
        public static ActionTypeId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();
    }
}
