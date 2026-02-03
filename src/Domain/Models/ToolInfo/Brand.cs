namespace Domain.Models.ToolInfo
{
    public class Brand
    {
        public BrandId Id { get; set; }
        public string Title { get; set; }

        public Brand(BrandId id, string title)
        {
            Id = id;
            Title = title;
        }
        public static Brand New(string title)
    => new(BrandId.New(), title);

        public void ChangeTitle(string title)
            => Title = title;
    }
}
