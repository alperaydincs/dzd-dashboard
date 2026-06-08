using DZDDashboard.Common.Validation;
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
               .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.Property(t => t.InstructorName)
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.Property(t => t.InstructorCompanyDetails)
               .HasMaxLength(ValidationConstants.MaxInstitutionLength);

        builder.Property(t => t.Details)
               .HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.Property(t => t.Location)
               .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.Property(t => t.IsActive)
               .HasDefaultValue(true);

        builder.HasIndex(t => t.Name).IsUnique();

        // Training→UserTrainings relationship configured in UserTrainingConfiguration — single source of truth

        builder.HasOne(t => t.ModifiedBy)
               .WithMany()
               .HasForeignKey(t => t.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}