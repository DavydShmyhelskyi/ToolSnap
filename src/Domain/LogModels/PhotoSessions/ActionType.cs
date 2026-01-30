namespace Domain.LogModels.PhotoSessions
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
            => new(ActionTypeId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
