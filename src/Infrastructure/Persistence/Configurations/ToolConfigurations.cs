using Domain.Models.ToolInfo;
using Domain.Models.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolConfiguration : IEntityTypeConfiguration<Tool>
{
    public void Configure(EntityTypeBuilder<Tool> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolId(x));

        builder.Property(x => x.ToolTypeId)
            .HasConversion(x => x.Value, x => new ToolTypeId(x))
            .IsRequired();
        builder.HasOne(x => x.ToolType)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.ToolTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.BrandId)
            .HasConversion(x => x.Value, x => new BrandId(x));
        builder.HasOne(x => x.Brand)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.ModelId)
            .HasConversion(x => x.Value, x => new ModelId(x));
        builder.HasOne(x => x.Model)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.ModelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.SerialNumber)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ToolStatusId)
            .HasConversion(x => x.Value, x => new ToolStatusId(x))
            .IsRequired();
        builder.HasOne(x => x.ToolStatus)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.ToolStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
