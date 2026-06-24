using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class SalaryHistoryConfiguration : IEntityTypeConfiguration<SalaryHistory>
{
    public void Configure(EntityTypeBuilder<SalaryHistory> builder)
    {
        builder.ToTable("UserSalaryHistories");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.NetAmount).HasPrecision(18, 2);
        builder.Property(s => s.GrossAmount).HasPrecision(18, 2);

        builder.Property(s => s.PayType)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(s => s.Currency)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);

        builder.Property(s => s.Period)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(s => s.PayrollCycle).HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.Property(s => s.Notes).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(s => s.StartDate).IsRequired();

        builder.HasIndex(s => new { s.UserId, s.StartDate });

        builder.HasOne(s => s.User)
               .WithMany(u => u.SalaryHistories)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.ModifiedBy)
               .WithMany()
               .HasForeignKey(s => s.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
