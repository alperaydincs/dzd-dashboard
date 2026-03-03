using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
    public void Configure(EntityTypeBuilder<Training> builder)
    {
        builder.ToTable("Trainings");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.InstructorName)
               .HasMaxLength(150);

        builder.Property(t => t.InstructorCompanyDetails)
               .HasMaxLength(300);

        builder.Property(t => t.Details)
               .HasMaxLength(1000);

        builder.Property(t => t.Location)
               .HasMaxLength(200);

        builder.Property(t => t.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(t => t.Name).IsUnique();

        builder.HasMany(t => t.UserTrainings)
               .WithOne(ut => ut.Training)
               .HasForeignKey(ut => ut.TrainingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.ModifiedBy)
               .WithMany()
               .HasForeignKey(t => t.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}