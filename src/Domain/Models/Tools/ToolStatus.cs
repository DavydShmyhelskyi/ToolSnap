namespace Domain.Models.Tools
{
    public class ToolStatus
    {
        public ToolStatusId Id { get; }
        public string Title { get; private set; }

        private ToolStatus(ToolStatusId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static ToolStatus New(string title)
            => new(ToolStatusId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
