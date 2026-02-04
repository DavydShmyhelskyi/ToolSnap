using Domain.Models.DetectedTools;
using Domain.Models.Tools;

namespace Domain.Models.ToolInfo
{
    public class Model
    {
        public ModelId Id { get; set; }
        public string Title { get; set; }

        // navigation properties
        public IEnumerable<Tool> Tools { get; private set; } = new List<Tool>();
        public IEnumerable<DetectedTool> DetectedTools { get; private set; } = new List<DetectedTool>();

        public Model(ModelId id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Model New(string title)
    => new(ModelId.New(), title.Trim().ToLower());

        public void Update(string title)
            => Title = title.Trim().ToLower();
    }
}
