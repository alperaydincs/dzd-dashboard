using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.Property(c => c.Description)
               .HasMaxLength(ValidationConstants.MaxDescriptionLength);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(c => c.Departments)
               .WithOne(d => d.Company)
               .HasForeignKey(d => d.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ModifiedBy)
               .WithMany()
               .HasForeignKey(c => c.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
