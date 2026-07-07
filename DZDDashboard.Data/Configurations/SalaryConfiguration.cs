using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("UserSalaries");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Amount).HasPrecision(18, 2);

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

        builder.HasIndex(s => s.UserId).IsUnique();

        builder.HasOne(s => s.User)
               .WithOne(u => u.Salary)
               .HasForeignKey<Salary>(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
