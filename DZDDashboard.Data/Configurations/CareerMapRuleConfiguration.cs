using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CareerMapRuleConfiguration : IEntityTypeConfiguration<CareerMapRule>
{
    public void Configure(EntityTypeBuilder<CareerMapRule> builder)
    {
        builder.ToTable("CareerMapRules");
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.CareerPathId, c.Grade }).IsUnique();

        builder.Property(c => c.ManagerPerformanceEvaluation).HasDefaultValue(false);
        builder.Property(c => c.AssessmentCenterApplication).HasDefaultValue(false);
        builder.Property(c => c.TechnicalInterview).HasDefaultValue(false);
        builder.Property(c => c.CaseStudy).HasDefaultValue(false);
        builder.Property(c => c.EnglishProficiency).HasDefaultValue(false);
        builder.Property(c => c.CommitteeApproval).HasDefaultValue(false);

        // ── Owned duration types ──────────────────────────────────────────────
        // Column names preserved from the original flat-column design so no data migration is needed.
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
