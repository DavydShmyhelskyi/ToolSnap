using Domain.Models.Tools;

namespace Domain.Models.ToolInfo
{
    public class ToolStatus
    {
        public ToolStatusId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IEnumerable<Tool> Tools { get; private set; } = new List<Tool>();

        private ToolStatus(ToolStatusId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static ToolStatus New(string title)
            => new(ToolStatusId.New(), title.Trim().ToLower());

        public void Update(string title)
            => Title = title.Trim().ToLower();
    }
}
