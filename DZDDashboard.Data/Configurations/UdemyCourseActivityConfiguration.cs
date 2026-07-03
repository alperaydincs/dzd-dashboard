using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UdemyCourseActivityConfiguration : IEntityTypeConfiguration<UdemyCourseActivity>
{
    public void Configure(EntityTypeBuilder<UdemyCourseActivity> builder)
    {
        builder.ToTable("UdemyCourseActivities");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserEmail)
               .HasMaxLength(ValidationConstants.MaxEmailLength)
               .IsRequired();

        builder.Property(a => a.UserExternalId)
               .HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(a => a.CourseTitle)
               .HasMaxLength(ValidationConstants.MaxProjectNameLength)
               .IsRequired();

        builder.Property(a => a.CourseCategory)
               .HasMaxLength(ValidationConstants.MaxNameLength);

        builder.Property(a => a.AssignedBy)
               .HasMaxLength(ValidationConstants.MaxFullNameLength);

        // One row per learner+course; lets the sync upsert by (UdemyUserId, CourseId).
        builder.HasIndex(a => new { a.UdemyUserId, a.CourseId }).IsUnique();

        builder.HasIndex(a => a.UserId);

        builder.HasOne(a => a.User)
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.ModifiedBy)
               .WithMany()
               .HasForeignKey(a => a.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
