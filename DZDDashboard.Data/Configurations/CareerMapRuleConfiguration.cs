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

        builder.HasIndex(c => new { c.JobId, c.Grade }).IsUnique();

        builder.Property(c => c.ManagerPerformanceEvaluation)
               .HasDefaultValue(false);

        builder.Property(c => c.AssesmentCenterApplication)
               .HasDefaultValue(false);

        builder.Property(c => c.TechnicalInterview)
               .HasDefaultValue(false);

        builder.Property(c => c.CaseStudy)
               .HasDefaultValue(false);

        builder.Property(c => c.EnglishProficiency)
               .HasDefaultValue(false);

        builder.Property(c => c.CommitteeApproval)
               .HasDefaultValue(false);

        builder.HasOne(c => c.Job)
               .WithMany()
               .HasForeignKey(c => c.JobId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ModifiedBy)
               .WithMany()
               .HasForeignKey(c => c.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}