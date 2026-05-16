namespace Domain.Models.ToolTransfers
{
    public sealed record ToolTransferId(Guid Value)
    {
        public static ToolTransferId Empty() => new(Guid.Empty);
        public static ToolTransferId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
