namespace Domain.Models.ToolInfo
{
    public record ToolStatusId(Guid Value)
    {
        public static ToolStatusId Empty() => new(Guid.Empty);
        public static ToolStatusId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();
    }
}
