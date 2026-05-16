namespace Domain.Models.ToolLiabilities
{
    public sealed record ToolLiabilityId(Guid Value)
    {
        public static ToolLiabilityId Empty() => new(Guid.Empty);
        public static ToolLiabilityId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
