using Domain.Models.Users;

namespace Domain.Models.PhotoSessions
{
    public class ActionType
    {
        public ActionTypeId Id { get; }
        public string Title { get; private set; }

        // navigation properties
        public IEnumerable<PhotoSession> PhotoSessions { get; private set; } = new List<PhotoSession>();

        private ActionType(ActionTypeId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static ActionType New(string title)
            => new(ActionTypeId.New(), title.Trim().ToLower());

        public void Update(string title)
            => Title = title.Trim().ToLower();
    }
}
