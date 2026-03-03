using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class DzdStatusConfiguration : IEntityTypeConfiguration<DzdStatus>
{
    public void Configure(EntityTypeBuilder<DzdStatus> builder)
    {
        builder.ToTable("DzdStatuses");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DzdStatusName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(d => d.DzdStatusName).IsUnique();

        builder.HasMany(d => d.Projects)
               .WithOne(p => p.DzdStatus)
               .HasForeignKey(p => p.DzdStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.ModifiedBy)
               .WithMany()
               .HasForeignKey(d => d.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}