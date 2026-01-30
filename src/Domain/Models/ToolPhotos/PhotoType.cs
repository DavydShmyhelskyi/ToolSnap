namespace Domain.Models.ToolPhotos
{
    public class PhotoType
    {
        public PhotoTypeId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IReadOnlyCollection<ToolPhoto> ToolPhotos => _photos;
        private readonly List<ToolPhoto> _photos = new();

        private PhotoType(PhotoTypeId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static PhotoType New(string title)
            => new(PhotoTypeId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
