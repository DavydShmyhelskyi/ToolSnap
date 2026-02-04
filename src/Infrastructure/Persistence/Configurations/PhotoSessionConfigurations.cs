using Domain.Models.PhotoSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PhotoSessionConfiguration : IEntityTypeConfiguration<PhotoSession>
{
    public void Configure(EntityTypeBuilder<PhotoSession> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new PhotoSessionId(x));

        builder.Property(x => x.Latitude)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ActionTypeId)
            .HasConversion(x => x.Value, x => new ActionTypeId(x))
            .IsRequired();
        builder.HasOne(x => x.ActionType)
            .WithMany(x => x.PhotoSessions)
            .HasForeignKey(x => x.ActionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
