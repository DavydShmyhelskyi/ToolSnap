namespace Domain.Models.Locations
{
    public class LocationType
    {
        public LocationTypeId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IReadOnlyCollection<Location> Locations => _locations;
        private readonly List<Location> _locations = new();

        private LocationType(LocationTypeId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static LocationType New(string title)
            => new(LocationTypeId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
