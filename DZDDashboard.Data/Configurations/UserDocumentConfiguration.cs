using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserDocumentConfiguration : IEntityTypeConfiguration<UserDocument>
{
    public void Configure(EntityTypeBuilder<UserDocument> builder)
    {
        builder.ToTable("UserDocuments");

        builder.HasKey(ud => ud.Id);

        builder.Property(ud => ud.FileName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(ud => ud.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(ud => ud.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(ud => new { ud.UserId, ud.FileName }).IsUnique();

        builder.HasOne(ud => ud.User)
               .WithMany()
               .HasForeignKey(ud => ud.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ud => ud.DocumentCategory)
               .WithMany(c => c.UserDocuments)
               .HasForeignKey(ud => ud.DocumentCategoryId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ud => ud.ModifiedBy)
               .WithMany()
               .HasForeignKey(ud => ud.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}