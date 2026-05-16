using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.ToolLiabilities;
using Domain.Models.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolLiabilityConfiguration : IEntityTypeConfiguration<ToolLiability>
{
    public void Configure(EntityTypeBuilder<ToolLiability> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolLiabilityId(x));

        builder.Property(x => x.ToolId)
            .HasConversion(x => x.Value, x => new ToolId(x))
            .IsRequired();
        builder.HasOne(x => x.Tool)
            .WithMany()
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ToolAssignmentId)
            .HasConversion(x => x.Value, x => new ToolAssignmentId(x))
            .IsRequired();
        builder.HasOne(x => x.ToolAssignment)
            .WithMany()
            .HasForeignKey(x => x.ToolAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PriceAtAssignment)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(x => x.AssignedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.ClosedAt)
            .HasConversion(new DateTimeUtcConverter());

        builder.Ignore(x => x.IsOpen);
    }
}
