namespace Domain.Models.PhotoSessions
{
    public class ActionType
    {
        public ActionTypeId Id { get; }
        public string Title { get; private set; }

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
