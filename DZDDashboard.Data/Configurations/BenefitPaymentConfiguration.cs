using DZDDashboard.Common.Constants;
using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BenefitPaymentConfiguration : IEntityTypeConfiguration<BenefitPayment>
{
    public void Configure(EntityTypeBuilder<BenefitPayment> builder)
    {
        builder.ToTable("UserBenefitRecords");
        builder.HasKey(x => x.Id);

        builder.HasDiscriminator(x => x.BenefitType)
            .HasValue<HealthInsuranceBenefit>(BenefitTypes.PrivateHealthInsurance)
            .HasValue<PensionBenefit>(BenefitTypes.PrivatePension)
            .HasValue<OtherBenefit>(BenefitTypes.Other);

        builder.Property(x => x.BenefitType).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.BenefitName).HasMaxLength(ValidationConstants.MaxBenefitNameLength);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);
        builder.Property(x => x.Period).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.ProviderName).HasMaxLength(ValidationConstants.MaxProviderNameLength);
        builder.Property(x => x.Notes).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.StartDate).IsRequired();

        builder.HasIndex(x => new { x.UserId, x.BenefitType, x.StartDate });

        builder.HasOne(x => x.User)
            .WithMany(u => u.BenefitPayments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
