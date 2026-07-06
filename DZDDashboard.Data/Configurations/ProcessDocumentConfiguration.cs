using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProcessDocumentConfiguration : IEntityTypeConfiguration<ProcessDocument>
{
    public void Configure(EntityTypeBuilder<ProcessDocument> builder)
    {
        builder.ToTable("ProcessDocuments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.FileName).HasMaxLength(ValidationConstants.MaxFileNameLength);
        builder.Property(x => x.ContentType).HasMaxLength(ValidationConstants.MaxContentTypeLength);

        builder.HasIndex(x => x.OnboardingProcessId);
        builder.HasIndex(x => x.OffboardingProcessId);

        builder.HasOne(x => x.OnboardingProcess)
            .WithMany(p => p.Documents)
            .HasForeignKey(x => x.OnboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.OffboardingProcess)
            .WithMany(p => p.Documents)
            .HasForeignKey(x => x.OffboardingProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.File)
            .WithMany()
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UploadedBy)
            .WithMany()
            .HasForeignKey(x => x.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReviewedBy)
            .WithMany()
            .HasForeignKey(x => x.ReviewedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
