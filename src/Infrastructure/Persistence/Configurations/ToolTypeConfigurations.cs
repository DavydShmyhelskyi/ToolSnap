using Domain.Models.ToolInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolTypeConfiguration : IEntityTypeConfiguration<ToolType>
{
    public void Configure(EntityTypeBuilder<ToolType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolTypeId(x));

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();
    }
}
