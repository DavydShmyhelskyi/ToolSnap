using System.Drawing;

namespace Domain.Models.ToolPhotos
{
    public class PhotoType //TOP,SIDE,BOTTOM,SERIAL_NUMBER
    {
        public PhotoTypeId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IEnumerable<ToolPhoto> ToolPhotos { get; private set; } = new List<ToolPhoto>();
        private PhotoType(PhotoTypeId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static PhotoType New(string title)
            => new(PhotoTypeId.New(), title.Trim().ToLower());

        public void Update(string title)
            => Title = title.Trim().ToLower();
    }
}
