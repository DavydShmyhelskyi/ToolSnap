using Domain.Models.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolStatusConfiguration : IEntityTypeConfiguration<ToolStatus>
{
    public void Configure(EntityTypeBuilder<ToolStatus> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolStatusId(x));

        builder.Property(x => x.Title)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(x => x.Title).IsUnique();
    }
}
