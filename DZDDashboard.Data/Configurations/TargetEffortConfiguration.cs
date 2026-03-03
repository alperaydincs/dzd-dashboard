using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class TargetEffortConfiguration : IEntityTypeConfiguration<TargetEffort>
{
    public void Configure(EntityTypeBuilder<TargetEffort> builder)
    {
        builder.ToTable("TargetEfforts");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Target)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.CompletedTarget)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.RemainingTarget)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.ProjectBonusAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.ItsmBonusAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.ManagerBonusEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.ManagerBonusAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.TotalBonusAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(t => t.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(t => new { t.PeriodId, t.UserId }).IsUnique();

        builder.HasOne(t => t.Period)
               .WithMany(p => p.TargetEfforts)
               .HasForeignKey(t => t.PeriodId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.User)
               .WithMany(u => u.TargetEfforts)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ModifiedBy)
               .WithMany()
               .HasForeignKey(t => t.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}