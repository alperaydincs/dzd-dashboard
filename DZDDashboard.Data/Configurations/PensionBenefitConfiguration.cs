using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class PensionBenefitConfiguration : IEntityTypeConfiguration<PensionBenefit>
{
    public void Configure(EntityTypeBuilder<PensionBenefit> builder)
    {
        builder.Property(x => x.EmployeeContributionAmount).HasPrecision(18, 2);
        builder.Property(x => x.EmployerContributionAmount).HasPrecision(18, 2);
        builder.Property(x => x.PolicyNumber).HasMaxLength(ValidationConstants.MaxPolicyNumberLength);
    }
}
