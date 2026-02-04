namespace Domain.Models.ToolInfo
{
    public class Model
    {
        public ModelId Id { get; set; }
        public string Title { get; set; }

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
