using Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new LocationId(x));

        builder.Property(x => x.Name)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Address)
            .HasColumnType("varchar(500)");

        builder.Property(x => x.Latitude)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.LocationTypeId)
            .HasConversion(x => x.Value, x => new LocationTypeId(x))
            .IsRequired();

        builder.HasOne(x => x.LocationType)
            .WithMany(x => x.Locations)
            .HasForeignKey(x => x.LocationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
