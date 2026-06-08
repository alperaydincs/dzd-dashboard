using DZDDashboard.Common.Validation;
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

        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.HasIndex(d => d.Name).IsUnique();

        // Parent relationship: deleting a Company cascades to its Departments (EF client-side cascade)
        builder.HasOne(d => d.Company)
               .WithMany(c => c.Departments)
               .HasForeignKey(d => d.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Projects)
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