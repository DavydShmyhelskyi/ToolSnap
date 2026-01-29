namespace Domain.Models.Tools
{
    public sealed record ToolId(Guid Value)
    {
        public static ToolId Empty() => new(Guid.Empty);
        public static ToolId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
