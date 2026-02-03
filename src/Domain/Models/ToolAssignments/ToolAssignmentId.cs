namespace Domain.Models.ToolAssignments
{
    public record ToolAssignmentId(Guid Value)
    {
        public static ToolAssignmentId Empty() => new(Guid.Empty);
        public static ToolAssignmentId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
