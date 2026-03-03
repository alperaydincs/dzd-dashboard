using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ResolutionConfiguration : IEntityTypeConfiguration<Resolution>
{
    public void Configure(EntityTypeBuilder<Resolution> builder)
    {
        builder.ToTable("Resolutions");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ResolutionName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(r => r.ResolutionName).IsUnique();

        builder.HasMany(r => r.Itsms)
               .WithOne(i => i.Resolution)
               .HasForeignKey(i => i.ResolutionId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.ModifiedBy)
               .WithMany()
               .HasForeignKey(r => r.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}