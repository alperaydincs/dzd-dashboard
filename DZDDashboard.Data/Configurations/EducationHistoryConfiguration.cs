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
            .HasMaxLength(100);

        builder.Property(x => x.Institution)
            .HasMaxLength(250);

        builder.Property(x => x.Program)
            .HasMaxLength(250);

        builder.Property(x => x.Status)
            .HasMaxLength(100);

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
