using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolPhotoConfiguration : IEntityTypeConfiguration<ToolPhoto>
{
    public void Configure(EntityTypeBuilder<ToolPhoto> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolPhotoId(x));

        builder.Property(x => x.ToolId)
            .HasConversion(x => x.Value, x => new ToolId(x));

        builder.Property(x => x.PhotoTypeId)
            .HasConversion(x => x.Value, x => new PhotoTypeId(x));

        builder.Property(x => x.OriginalName)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.HasOne(x => x.Tool)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.ToolId);

        builder.HasOne(x => x.PhotoType)
            .WithMany(x => x.ToolPhotos)
            .HasForeignKey(x => x.PhotoTypeId);
    }
}
