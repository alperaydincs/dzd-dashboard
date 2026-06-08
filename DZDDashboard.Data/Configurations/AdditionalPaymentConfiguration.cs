using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class AdditionalPaymentConfiguration : IEntityTypeConfiguration<AdditionalPayment>
{
    public void Configure(EntityTypeBuilder<AdditionalPayment> builder)
    {
        builder.ToTable("UserAdditionalPayments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentType).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(ValidationConstants.MaxCurrencyCodeLength);
        builder.Property(x => x.Period).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Description).HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.HasIndex(x => new { x.UserId, x.Period });

        builder.HasOne(x => x.User)
            .WithMany(u => u.AdditionalPayments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
