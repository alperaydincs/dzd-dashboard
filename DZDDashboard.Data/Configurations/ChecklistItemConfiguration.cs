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

        builder.Property(x => x.StepKey).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.BenefitKind).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Note).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(x => x.DocumentFileName).HasMaxLength(ValidationConstants.MaxFileNameLength);
        builder.Property(x => x.DocumentContentType).HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(x => x.ProviderName).HasMaxLength(ValidationConstants.MaxProviderNameLength);
        builder.Property(x => x.Currency).HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);
        builder.Property(x => x.EmployeeAmount).HasPrecision(18, 2);
        builder.Property(x => x.EmployerAmount).HasPrecision(18, 2);

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

        builder.HasOne(x => x.DocumentStoredFile)
            .WithMany()
            .HasForeignKey(x => x.DocumentStoredFileId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Dependents)
            .WithOne(d => d.ChecklistItem)
            .HasForeignKey(d => d.ChecklistItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ChecklistItemDependentConfiguration : IEntityTypeConfiguration<ChecklistItemDependent>
{
    public void Configure(EntityTypeBuilder<ChecklistItemDependent> builder)
    {
        builder.ToTable("ChecklistItemDependents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DependentName).HasMaxLength(ValidationConstants.MaxFullNameLength);
        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.HasOne(x => x.DependentTypeRef)
            .WithMany()
            .HasForeignKey(x => x.DependentTypeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
