using Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LocationTypeConfiguration : IEntityTypeConfiguration<LocationType>
{
    public void Configure(EntityTypeBuilder<LocationType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new LocationTypeId(x));

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();
    }
}
