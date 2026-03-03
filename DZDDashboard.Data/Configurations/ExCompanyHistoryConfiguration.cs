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
               .HasMaxLength(200);

        builder.Property(e => e.JobTitle)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(e => e.StartDate)
               .IsRequired();

        builder.HasOne(e => e.User)
               .WithMany(u => u.ExCompanyHistories)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ModifiedBy)
               .WithMany()
               .HasForeignKey(e => e.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}