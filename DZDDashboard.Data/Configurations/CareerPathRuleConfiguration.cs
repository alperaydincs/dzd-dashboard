using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CareerPathRuleConfiguration : IEntityTypeConfiguration<CareerPathRule>
{
    public void Configure(EntityTypeBuilder<CareerPathRule> builder)
    {
        builder.ToTable("CareerPathRules");
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.CareerPathId, c.Grade }).IsUnique();

        builder.Property(c => c.ManagerPerformanceEvaluation).HasDefaultValue(false);
        builder.Property(c => c.AssessmentCenterApplication).HasDefaultValue(false);
        builder.Property(c => c.TechnicalInterview).HasDefaultValue(false);
        builder.Property(c => c.CaseStudy).HasDefaultValue(false);
        builder.Property(c => c.EnglishProficiency).HasDefaultValue(false);
        builder.Property(c => c.CommitteeApproval).HasDefaultValue(false);

        builder.OwnsOne(c => c.MinRoleTime, b =>
        {
            b.Property(d => d.Months).HasColumnName("MinRoleTimeMonth");
            b.Property(d => d.Years).HasColumnName("MinRoleTimeYear");
        });

        builder.OwnsOne(c => c.MinExperience, b =>
        {
            b.Property(d => d.Months).HasColumnName("MinExperienceMonth");
            b.Property(d => d.Years).HasColumnName("MinExperienceYear");
        });

        builder.Property(c => c.SalaryIncreasePercent).HasPrecision(5, 2);

        builder.Property(c => c.PrivatePensionInsuranceAmount).HasPrecision(18, 2);
        builder.Property(c => c.PrivatePensionInsuranceCurrency).HasMaxLength(3);

        builder.Property(c => c.EmployerContributionUpperLimitAmount).HasPrecision(18, 2);
        builder.Property(c => c.EmployerContributionUpperLimitCurrency).HasMaxLength(3);

        builder.Property(c => c.MealAllowanceAmount).HasPrecision(18, 2);
        builder.Property(c => c.MealAllowanceCurrency).HasMaxLength(3);

        builder.HasOne(c => c.CareerPath)
               .WithMany(p => p.Rules)
               .HasForeignKey(c => c.CareerPathId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ModifiedBy)
               .WithMany()
               .HasForeignKey(c => c.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
