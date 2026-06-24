using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ExCompanyHistoryConfiguration : IEntityTypeConfiguration<ExCompanyHistory>
{
    public void Configure(EntityTypeBuilder<ExCompanyHistory> builder)
    {
        builder.ToTable("ExCompanyHistories");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyName)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.Property(e => e.JobTitle)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.Property(e => e.StartDate)
               .IsRequired();

        builder.HasIndex(e => e.UserId).HasDatabaseName("IX_ExCompanyHistories_UserId");

        builder.HasOne(e => e.ModifiedBy)
               .WithMany()
               .HasForeignKey(e => e.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}