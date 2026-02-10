using Domain.Models.ToolPhotos;
using Domain.Models.Tools;
using Infrastructure.Persistence.Converters;
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

        builder.Property(x => x.OriginalName)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(x => x.UploadDate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.ToolId)
            .HasConversion(x => x.Value, x => new ToolId(x))
            .IsRequired();
        builder.HasOne(x => x.Tool)
            .WithMany(x => x.ToolPhotos)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PhotoTypeId)
            .HasConversion(x => x.Value, x => new PhotoTypeId(x))
            .IsRequired();
        builder.HasOne(x => x.PhotoType)
            .WithMany(x => x.ToolPhotos)
            .HasForeignKey(x => x.PhotoTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
