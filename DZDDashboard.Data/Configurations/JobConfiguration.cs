using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(j => j.Title).IsUnique();

        builder.HasMany(j => j.Users)
               .WithOne(u => u.Job)
               .HasForeignKey(u => u.JobId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(j => j.HeadLeadCoefficients)
               .WithOne(h => h.Job)
               .HasForeignKey(h => h.JobId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(j => j.ModifiedBy)
               .WithMany()
               .HasForeignKey(j => j.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}