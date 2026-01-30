namespace Domain.Models.Locations
{
    public record LocationTypeId(Guid Value)
    {
        public static LocationTypeId Empty() => new(Guid.Empty);
        public static LocationTypeId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();
    }
}
