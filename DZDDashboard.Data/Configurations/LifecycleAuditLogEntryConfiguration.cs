using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class LifecycleAuditLogEntryConfiguration : IEntityTypeConfiguration<LifecycleAuditLogEntry>
{
    public void Configure(EntityTypeBuilder<LifecycleAuditLogEntry> builder)
    {
        builder.ToTable("LifecycleAuditLogEntries");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Detail).IsRequired().HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.HasIndex(x => x.OnboardingProcessId);
        builder.HasIndex(x => x.OffboardingProcessId);

        builder.HasOne(x => x.OnboardingProcess)
            .WithMany(p => p.AuditLog)
            .HasForeignKey(x => x.OnboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.OffboardingProcess)
            .WithMany(p => p.AuditLog)
            .HasForeignKey(x => x.OffboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PerformedBy)
            .WithMany()
            .HasForeignKey(x => x.PerformedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
