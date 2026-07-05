using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class OnboardingProcessConfiguration : IEntityTypeConfiguration<OnboardingProcess>
{
    public void Configure(EntityTypeBuilder<OnboardingProcess> builder)
    {
        builder.ToTable("OnboardingProcesses");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.StartDate).IsRequired();

        builder.HasOne(x => x.User)
            .WithOne(u => u.OnboardingProcess)
            .HasForeignKey<OnboardingProcess>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Manager)
            .WithMany()
            .HasForeignKey(x => x.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(i => i.OnboardingProcess)
            .HasForeignKey(i => i.OnboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OffboardingProcessConfiguration : IEntityTypeConfiguration<OffboardingProcess>
{
    public void Configure(EntityTypeBuilder<OffboardingProcess> builder)
    {
        builder.ToTable("OffboardingProcesses");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Type).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.ExitDate).IsRequired();

        builder.HasOne(x => x.User)
            .WithOne(u => u.OffboardingProcess)
            .HasForeignKey<OffboardingProcess>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(i => i.OffboardingProcess)
            .HasForeignKey(i => i.OffboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
