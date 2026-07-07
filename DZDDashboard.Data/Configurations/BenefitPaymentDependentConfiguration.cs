using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BenefitPaymentDependentConfiguration : IEntityTypeConfiguration<BenefitPaymentDependent>
{
    public void Configure(EntityTypeBuilder<BenefitPaymentDependent> builder)
    {
        builder.ToTable("UserBenefitDependents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DependentName).HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.StartDate).IsRequired();

        builder.Property(x => x.RelationType).HasMaxLength(ValidationConstants.MaxStandardLength);
    }
}
