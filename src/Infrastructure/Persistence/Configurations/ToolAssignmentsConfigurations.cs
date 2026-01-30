using Domain.Models.Locations;
using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.Users;
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

        builder.Property(x => x.ToolId)
            .HasConversion(x => x.Value, x => new ToolId(x));

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.LocationId)
            .HasConversion(x => x.Value, x => new LocationId(x));

        builder.HasOne(x => x.Tool)
            .WithMany(x => x.Assignments)
            .HasForeignKey(x => x.ToolId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ToolAssignments)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Location)
            .WithMany(x => x.ToolAssignments)
            .HasForeignKey(x => x.LocationId);
    }
}
