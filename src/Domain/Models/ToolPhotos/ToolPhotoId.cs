namespace Domain.Models.ToolPhotos
{
    public record ToolPhotoId(Guid Value)
    {
        public static ToolPhotoId Empty() => new(Guid.Empty);
        public static ToolPhotoId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
