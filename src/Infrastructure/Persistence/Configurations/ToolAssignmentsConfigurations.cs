using Domain.Models.ToolPhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class ToolAssignmentConfigurations : IEntityTypeConfiguration<ToolPhoto>
    {
        public void Configure(EntityTypeBuilder<ToolPhoto> builder)
        {
        }
    }
}
