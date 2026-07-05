using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class PositionHistoryConfiguration : IEntityTypeConfiguration<PositionHistory>
{
    public void Configure(EntityTypeBuilder<PositionHistory> builder)
    {
        builder.ToTable("UserPositionHistories");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.JobTitle).HasMaxLength(200);
        builder.Property(p => p.CompanyName).HasMaxLength(200);
        builder.Property(p => p.ChangeType).HasMaxLength(100);
        builder.Property(p => p.DepartmentName).HasMaxLength(ValidationConstants.MaxEntityNameLength);
        builder.Property(p => p.TeamName).HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.HasOne(p => p.User)
               .WithMany(u => u.PositionHistories)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Department)
               .WithMany()
               .HasForeignKey(p => p.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Team)
               .WithMany()
               .HasForeignKey(p => p.TeamId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
