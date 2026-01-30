using Domain.Models.ToolPhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ToolPhotoConfigurations : IEntityTypeConfiguration<ToolPhoto>
    {
        public void Configure(EntityTypeBuilder<ToolPhoto> builder)
        {
        }
    }
}
