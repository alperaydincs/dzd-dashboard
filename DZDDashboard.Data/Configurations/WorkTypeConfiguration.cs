using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
{
    public void Configure(EntityTypeBuilder<WorkType> builder)
    {
        builder.ToTable("WorkTypes");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxNameLength);

        builder.Property(w => w.Description)
               .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasOne(w => w.ModifiedBy)
               .WithMany()
               .HasForeignKey(w => w.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
