using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserCvDocumentConfiguration : IEntityTypeConfiguration<UserCvDocument>
{
    public void Configure(EntityTypeBuilder<UserCvDocument> builder)
    {
        builder.ToTable("UserCvDocuments");

        builder.HasKey(ud => ud.Id);

        builder.Property(ud => ud.FileName)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxFileNameLength);

        builder.Property(ud => ud.ContentType)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxContentTypeLength);

        builder.Property(ud => ud.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(ud => new { ud.UserId, ud.FileName }).IsUnique();

        builder.HasOne(ud => ud.User)
               .WithMany()
               .HasForeignKey(ud => ud.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ud => ud.File)
               .WithMany()
               .HasForeignKey(ud => ud.FileId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
