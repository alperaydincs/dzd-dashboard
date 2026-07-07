using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("UserPositions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.JobTitle).HasMaxLength(200);
        builder.Property(p => p.CompanyName).HasMaxLength(200);
        builder.Property(p => p.ChangeType).HasMaxLength(100);
        builder.Property(p => p.DepartmentName).HasMaxLength(ValidationConstants.MaxEntityNameLength);
        builder.Property(p => p.TeamName).HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.HasIndex(p => p.UserId).IsUnique();

        builder.HasOne(p => p.User)
               .WithOne(u => u.Position)
               .HasForeignKey<Position>(p => p.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
