using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("UserEducations");

        builder.HasKey(x => x.Id);

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

        builder.Property(x => x.EducationLevel).HasMaxLength(ValidationConstants.MaxStandardLength);
    }
}
