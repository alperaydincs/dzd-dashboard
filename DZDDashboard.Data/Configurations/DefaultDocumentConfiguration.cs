using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class DefaultDocumentConfiguration : IEntityTypeConfiguration<DefaultDocument>
{
    public void Configure(EntityTypeBuilder<DefaultDocument> builder)
    {
        builder.ToTable("DefaultDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(d => d.Content)
               .IsRequired();

        builder.Property(d => d.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(d => d.DocumentName).IsUnique();

        builder.HasOne(d => d.ModifiedBy)
               .WithMany()
               .HasForeignKey(d => d.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}