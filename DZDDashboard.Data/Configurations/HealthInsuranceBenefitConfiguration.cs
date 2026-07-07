using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class HealthInsuranceBenefitConfiguration : IEntityTypeConfiguration<HealthInsuranceBenefit>
{
    public void Configure(EntityTypeBuilder<HealthInsuranceBenefit> builder)
    {
        builder.HasMany(x => x.Dependents)
            .WithOne(d => d.BenefitPayment)
            .HasForeignKey(d => d.BenefitPaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
