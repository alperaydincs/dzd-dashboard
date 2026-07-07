using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CareerPathConfiguration : IEntityTypeConfiguration<CareerPath>
{
    public void Configure(EntityTypeBuilder<CareerPath> builder)
    {
        builder.ToTable("CareerPaths");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxStandardLength);
    }
}
