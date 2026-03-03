using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class PeriodConfiguration : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        builder.ToTable("Periods");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PeriodName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Active)
               .HasDefaultValue(false);

        builder.HasIndex(p => p.PeriodName).IsUnique();

        builder.HasMany(p => p.TargetEfforts)
               .WithOne(t => t.Period)
               .HasForeignKey(t => t.PeriodId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Itsms)
               .WithOne(i => i.Period)
               .HasForeignKey(i => i.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Projects)
               .WithOne(pr => pr.Period)
               .HasForeignKey(pr => pr.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.HeadLeadCoefficients)
               .WithOne(h => h.Period)
               .HasForeignKey(h => h.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.ModifiedBy)
               .WithMany()
               .HasForeignKey(p => p.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}