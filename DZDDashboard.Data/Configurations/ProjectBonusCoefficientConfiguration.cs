using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProjectBonusCoefficientConfiguration : IEntityTypeConfiguration<ProjectBonusCoefficient>
{
    public void Configure(EntityTypeBuilder<ProjectBonusCoefficient> builder)
    {
        builder.ToTable("ProjectBonusCoefficients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Coefficient)
               .IsRequired()
               .HasColumnType("decimal(5, 2)");

        builder.HasIndex(p => p.PeriodId).IsUnique();

        builder.HasOne(p => p.Period)
               .WithMany()
               .HasForeignKey(p => p.PeriodId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ModifiedBy)
               .WithMany()
               .HasForeignKey(p => p.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}