using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class JiraStatusConfiguration : IEntityTypeConfiguration<JiraStatus>
{
    public void Configure(EntityTypeBuilder<JiraStatus> builder)
    {
        builder.ToTable("JiraStatuses");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.JiraStatusName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(j => j.JiraStatusName).IsUnique();

        builder.HasMany(j => j.Projects)
               .WithOne(p => p.JiraStatus)
               .HasForeignKey(p => p.JiraStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(j => j.ModifiedBy)
               .WithMany()
               .HasForeignKey(j => j.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}