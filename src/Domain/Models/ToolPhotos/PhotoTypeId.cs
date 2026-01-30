namespace Domain.Models.ToolPhotos
{
    public record PhotoTypeId(Guid Value)
    {
        public static PhotoTypeId Empty() => new(Guid.Empty);
        public static PhotoTypeId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();
    }
}
