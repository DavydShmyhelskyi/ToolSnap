namespace Domain.Models.ToolAssignments
{
    public sealed record ToolAssignmentId(Guid Value)
    {
        public static ToolAssignmentId Empty() => new(Guid.Empty);
        public static ToolAssignmentId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
