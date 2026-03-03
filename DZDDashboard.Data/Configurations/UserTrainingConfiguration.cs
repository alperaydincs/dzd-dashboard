using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserTrainingConfiguration : IEntityTypeConfiguration<UserTraining>
{
    public void Configure(EntityTypeBuilder<UserTraining> builder)
    {
        builder.ToTable("UserTrainings");

        builder.HasKey(ut => ut.Id);

        builder.Property(ut => ut.Status)
               .HasMaxLength(100);

        builder.Property(ut => ut.Description)
               .HasMaxLength(500);

        builder.Property(ut => ut.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(ut => new { ut.UserId, ut.TrainingId }).IsUnique();

        builder.HasOne(ut => ut.Training)
               .WithMany(t => t.UserTrainings)
               .HasForeignKey(ut => ut.TrainingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ut => ut.User)
               .WithMany(u => u.UserTrainings)
               .HasForeignKey(ut => ut.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ut => ut.ModifiedBy)
               .WithMany()
               .HasForeignKey(ut => ut.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}