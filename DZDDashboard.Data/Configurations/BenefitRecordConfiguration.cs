using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BenefitRecordConfiguration : IEntityTypeConfiguration<BenefitRecord>
{
    public void Configure(EntityTypeBuilder<BenefitRecord> builder)
    {
        builder.ToTable("UserBenefitRecords");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BenefitType).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Payer).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);
        builder.Property(x => x.Period).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Source).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.ReferenceId).HasMaxLength(ValidationConstants.MaxReferenceCodeLength);
        builder.Property(x => x.ProviderName).HasMaxLength(ValidationConstants.MaxProviderNameLength);
        builder.Property(x => x.Notes).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.StartDate).IsRequired();

        builder.HasIndex(x => new { x.UserId, x.BenefitType, x.StartDate });

        builder.HasOne(x => x.User)
            .WithMany(u => u.BenefitRecords)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Dependents)
            .WithOne(d => d.BenefitRecord)
            .HasForeignKey(d => d.BenefitRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
