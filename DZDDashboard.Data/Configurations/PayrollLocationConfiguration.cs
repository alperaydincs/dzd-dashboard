using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class PayrollLocationConfiguration : IEntityTypeConfiguration<PayrollLocation>
{
    public void Configure(EntityTypeBuilder<PayrollLocation> builder)
    {
        builder.ToTable("PayrollLocations");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Location)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(p => p.Location).IsUnique();

        builder.HasMany(p => p.IntertechTeams)
               .WithOne(s => s.PayrollLocation)
               .HasForeignKey(s => s.PayrollLocationId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.ModifiedBy)
               .WithMany()
               .HasForeignKey(p => p.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}