using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class HeadLeadCoefficientConfiguration : IEntityTypeConfiguration<HeadLeadCoefficient>
{
    public void Configure(EntityTypeBuilder<HeadLeadCoefficient> builder)
    {
        builder.ToTable("HeadLeadCoefficients");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Coefficient)
               .IsRequired()
               .HasColumnType("decimal(5, 2)");

        builder.HasIndex(h => new { h.PeriodId, h.JobId }).IsUnique();

        builder.HasOne(h => h.Period)
               .WithMany()
               .HasForeignKey(h => h.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(h => h.Job)
               .WithMany()
               .HasForeignKey(h => h.JobId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(h => h.ModifiedBy)
               .WithMany()
               .HasForeignKey(h => h.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}