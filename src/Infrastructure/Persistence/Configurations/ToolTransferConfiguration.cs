using Domain.Models.ToolAssignments;
using Domain.Models.Tools;
using Domain.Models.ToolTransfers;
using Domain.Models.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ToolTransferConfiguration : IEntityTypeConfiguration<ToolTransfer>
{
    public void Configure(EntityTypeBuilder<ToolTransfer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ToolTransferId(x));

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

        builder.Property(x => x.FromUserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.HasOne(x => x.FromUser)
            .WithMany()
            .HasForeignKey(x => x.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ToUserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.HasOne(x => x.ToUser)
            .WithMany()
            .HasForeignKey(x => x.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.InitiatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.RespondedAt)
            .HasConversion(new DateTimeUtcConverter());
    }
}
