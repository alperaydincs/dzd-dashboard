using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DepartmentName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(d => d.DepartmentName).IsUnique();

        builder.HasMany(d => d.Project)
               .WithOne(p => p.Department)
               .HasForeignKey(p => p.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Users)
               .WithOne(u => u.Department)
               .HasForeignKey(u => u.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.ModifiedBy)
               .WithMany()
               .HasForeignKey(d => d.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}