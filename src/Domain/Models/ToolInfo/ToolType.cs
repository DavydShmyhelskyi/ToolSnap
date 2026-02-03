namespace Domain.Models.ToolInfo
{
    public class ToolType
    {
        public ToolTypeId Id { get; set; }
        public string Title { get; set; }

        public ToolType(ToolTypeId id, string title)
        {
            Id = id;
            Title = title;
        }
        public static ToolType New(string title)
    => new(ToolTypeId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
