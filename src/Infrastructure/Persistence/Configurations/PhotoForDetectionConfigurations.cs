using Domain.Models.PhotoSessions;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class PhotoForDetectionConfiguration : IEntityTypeConfiguration<PhotoForDetection>
{
    public void Configure(EntityTypeBuilder<PhotoForDetection> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new PhotoForDetectionId(x));

        builder.Property(x => x.OriginalName)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(x => x.UploadDate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.PhotoSessionId)
            .HasConversion(x => x.Value, x => new PhotoSessionId(x))
            .IsRequired();
        builder.HasOne(x => x.PhotoSession)
            .WithMany(x => x.PhotosForDetection)
            .HasForeignKey(x => x.PhotoSessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
