using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserDocumentCategoryConfiguration : IEntityTypeConfiguration<UserDocumentCategory>
{
    public void Configure(EntityTypeBuilder<UserDocumentCategory> builder)
    {
        builder.ToTable("UserDocumentCategories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(c => c.Description)
               .HasMaxLength(500);

        builder.Property(c => c.ContentType)
               .HasMaxLength(100);

        builder.Property(c => c.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(c => c.UserDocuments)
               .WithOne(ud => ud.DocumentCategory)
               .HasForeignKey(ud => ud.DocumentCategoryId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.ModifiedBy)
               .WithMany()
               .HasForeignKey(c => c.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}