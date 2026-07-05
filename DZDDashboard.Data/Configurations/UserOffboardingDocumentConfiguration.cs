using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserOffboardingDocumentConfiguration : IEntityTypeConfiguration<UserOffboardingDocument>
{
    public void Configure(EntityTypeBuilder<UserOffboardingDocument> builder)
    {
        builder.ToTable("UserOffboardingDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FileName).HasMaxLength(ValidationConstants.MaxFileNameLength);
        builder.Property(d => d.ContentType).HasMaxLength(ValidationConstants.MaxContentTypeLength);

        builder.HasIndex(d => d.ChecklistItemId).IsUnique();

        builder.HasOne(d => d.ChecklistItem)
               .WithOne(c => c.OffboardingDocument)
               .HasForeignKey<UserOffboardingDocument>(d => d.ChecklistItemId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.File)
               .WithMany()
               .HasForeignKey(d => d.FileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ModifiedBy)
               .WithMany()
               .HasForeignKey(d => d.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
