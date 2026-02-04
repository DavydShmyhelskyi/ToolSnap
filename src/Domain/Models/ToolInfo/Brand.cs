using Domain.Models.Tools;
using Domain.Models.DetectedTools;

namespace Domain.Models.ToolInfo
{
    public class Brand
    {
        public BrandId Id { get; set; }
        public string Title { get; set; }

        // navigation properties
        public IEnumerable<Tool> Tools { get; private set; } = new List<Tool>();
        public IEnumerable<DetectedTool> DetectedTools { get; private set; } = new List<DetectedTool>();

        public Brand(BrandId id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Brand New(string title)
    => new(BrandId.New(), title.Trim().ToLower());

        public void Update(string title)
            => Title = title.Trim().ToLower();
    }
}
