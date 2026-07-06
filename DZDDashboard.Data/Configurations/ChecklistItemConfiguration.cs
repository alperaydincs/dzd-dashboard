using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> builder)
    {
        builder.ToTable("ChecklistItems");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.HasIndex(x => x.OnboardingProcessId);
        builder.HasIndex(x => x.OffboardingProcessId);

        builder.HasOne(x => x.CompletedBy)
            .WithMany()
            .HasForeignKey(x => x.CompletedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
