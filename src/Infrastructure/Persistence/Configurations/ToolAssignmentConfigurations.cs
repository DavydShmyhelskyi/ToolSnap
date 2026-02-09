using Domain.Models.DetectedTools;
using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolAssignmentConfiguration : IEntityTypeConfiguration<ToolAssignment>
{
    public void Configure(EntityTypeBuilder<ToolAssignment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolAssignmentId(x));

        builder.Property(x => x.TakenDetectedToolId)
            .HasConversion(x => x.Value, x => new DetectedToolId(x))
            .IsRequired();
        builder.HasOne(x => x.TakenDetectedTool)
            .WithMany()
            .HasForeignKey(x => x.TakenDetectedToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ReturnedDetectedToolId)
            .HasConversion(x => x.Value, x => new DetectedToolId(x));
        builder.HasOne(x => x.ReturnedDetectedTool)
            .WithMany()
            .HasForeignKey(x => x.ReturnedDetectedToolId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.ToolId)
            .HasConversion(x => x.Value, x => new ToolId(x))
            .IsRequired();
        builder.HasOne(x => x.Tool)
            .WithMany(x => x.ToolAssignments)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.ToolAssignments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.TakenLocationId)
            .HasConversion(x => x.Value, x => new LocationId(x))
            .IsRequired();
        builder.HasOne(x => x.TakenLocation)
            .WithMany(x => x.ToolAssignments)
            .HasForeignKey(x => x.TakenLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ReturnedLocationId)
            .HasConversion(x => x.Value, x => new LocationId(x));
        builder.HasOne(x => x.ReturnedLocation)
            .WithMany()
            .HasForeignKey(x => x.ReturnedLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.TakenAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();
        builder.Property(x => x.ReturnedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
    }
}