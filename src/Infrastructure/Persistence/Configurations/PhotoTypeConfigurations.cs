using Domain.Models.ToolPhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PhotoTypeConfiguration : IEntityTypeConfiguration<PhotoType>
{
    public void Configure(EntityTypeBuilder<PhotoType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new PhotoTypeId(x));

        builder.Property(x => x.Title)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(x => x.Title).IsUnique();
    }
}
