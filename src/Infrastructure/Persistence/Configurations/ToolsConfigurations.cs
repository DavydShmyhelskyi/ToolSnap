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
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.BrandId)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.ModelId)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.SerialNumber)
            .HasColumnType("varchar(255)");

        builder.HasOne(x => x.ToolStatus)
            .WithMany(x => x.Tools)
            .HasForeignKey(x => x.ToolStatusId);
    }
}
