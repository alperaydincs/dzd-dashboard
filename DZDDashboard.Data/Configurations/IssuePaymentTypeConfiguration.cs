using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class IssuePaymentTypeConfiguration : IEntityTypeConfiguration<IssuePaymentType>
{
    public void Configure(EntityTypeBuilder<IssuePaymentType> builder)
    {
        builder.ToTable("IssuePaymentTypes");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.PaymentTypeName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(i => i.Coefficient)
               .IsRequired()
               .HasColumnType("decimal(5, 2)");

        builder.HasIndex(i => new { i.PaymentTypeName, i.PeriodId }).IsUnique();

        builder.HasOne(i => i.Period)
               .WithMany()
               .HasForeignKey(i => i.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(i => i.Itsms)
               .WithOne(it => it.ItsmPaymentType)
               .HasForeignKey(it => it.ItsmPaymentTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ModifiedBy)
               .WithMany()
               .HasForeignKey(i => i.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}