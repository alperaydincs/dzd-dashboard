using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Level)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxGradeLevelLength);

        builder.Property(g => g.Currency)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);

        builder.Property(g => g.MinSalary)
               .HasColumnType("decimal(18,2)");

        builder.Property(g => g.MaxSalary)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(g => g.NextStep)
               .WithMany()
               .HasForeignKey(g => g.NextStepId)
               .OnDelete(DeleteBehavior.Restrict);

        // Grade does not inherit AuditableEntity — no ModifiedBy relationship.
    }
}
