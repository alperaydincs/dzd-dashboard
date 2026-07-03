using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class DeductionConfiguration : IEntityTypeConfiguration<Deduction>
{
    public void Configure(EntityTypeBuilder<Deduction> builder)
    {
        builder.ToTable("UserDeductions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Currency).IsRequired().HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);
        builder.Property(x => x.Period).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Notes).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.HasIndex(x => new { x.UserId, x.StartDate });

        builder.HasOne(x => x.User)
            .WithMany(u => u.Deductions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DeductionType).HasMaxLength(ValidationConstants.MaxStandardLength);
    }
}
