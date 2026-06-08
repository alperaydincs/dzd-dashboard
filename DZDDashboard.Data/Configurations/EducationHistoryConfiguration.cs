using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class EducationHistoryConfiguration : IEntityTypeConfiguration<EducationHistory>
{
    public void Configure(EntityTypeBuilder<EducationHistory> builder)
    {
        builder.ToTable("UserEducationHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Level)
            .HasMaxLength(ValidationConstants.MaxEducationLevelLength);

        builder.Property(x => x.Institution)
            .HasMaxLength(ValidationConstants.MaxInstitutionLength);

        builder.Property(x => x.Program)
            .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.Property(x => x.Status)
            .HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.HasOne(x => x.User)
            .WithMany(u => u.EducationHistories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
