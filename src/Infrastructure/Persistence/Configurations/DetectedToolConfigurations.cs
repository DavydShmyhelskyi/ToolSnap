using Domain.Models.DetectedTools;
using Domain.Models.PhotoSessions;
using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class DetectedToolConfiguration : IEntityTypeConfiguration<DetectedTool>
{
    public void Configure(EntityTypeBuilder<DetectedTool> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new DetectedToolId(x));

        builder.Property(x => x.PhotoSessionId)
            .HasConversion(x => x.Value, x => new PhotoSessionId(x))
            .IsRequired();
        builder.HasOne(x => x.PhotoSession)
            .WithMany(x => x.DetectedTools)
            .HasForeignKey(x => x.PhotoSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ToolTypeId)
            .HasConversion(x => x.Value, x => new ToolTypeId(x))
            .IsRequired();
        builder.HasOne(x => x.ToolType)
            .WithMany(x => x.DetectedTools)
            .HasForeignKey(x => x.ToolTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.BrandId)
            .HasConversion(x => x.Value, x => new BrandId(x));
        builder.HasOne(x => x.Brand)
            .WithMany(x => x.DetectedTools)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.ModelId)
            .HasConversion(x => x.Value, x => new ModelId(x));
        builder.HasOne(x => x.Model)
            .WithMany(x => x.DetectedTools)
            .HasForeignKey(x => x.ModelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.SerialNumber)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.Confidence)
            .IsRequired();

        builder.Property(x => x.RedFlagged)
            .IsRequired();
    }
}
