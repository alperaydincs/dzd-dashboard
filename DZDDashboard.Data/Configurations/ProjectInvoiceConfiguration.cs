using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProjectInvoiceConfiguration : IEntityTypeConfiguration<ProjectInvoice>
{
    public void Configure(EntityTypeBuilder<ProjectInvoice> builder)
    {
        builder.ToTable("ProjectInvoices");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProjectName)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxProjectNameLength);

        builder.Property(p => p.PurchaseInvoiceNumber)
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(p => p.PurchaseOrder)
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(p => p.EFaturaNumber)
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(p => p.JiraProjectNo)
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(p => p.JiraTaskNo)
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(p => p.Notes)
               .HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(p => p.TotalEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.TotalAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.UnitEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Vat)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.VatIncludedAmount)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Active)
               .HasDefaultValue(true);

        builder.Property(p => p.PartialInvoice)
               .HasDefaultValue(false);

        builder.HasIndex(p => p.PurchaseInvoiceNumber).IsUnique();
        builder.HasIndex(p => p.EFaturaNumber).IsUnique();

        builder.HasOne(p => p.Bank)
               .WithMany()
               .HasForeignKey(p => p.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.DzdStatus)
               .WithMany()
               .HasForeignKey(p => p.DzdStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Department)
               .WithMany()
               .HasForeignKey(p => p.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Period)
               .WithMany()
               .HasForeignKey(p => p.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.PayrollLocation)
               .WithMany()
               .HasForeignKey(p => p.PayrollLocationId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.ModifiedBy)
               .WithMany()
               .HasForeignKey(p => p.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
