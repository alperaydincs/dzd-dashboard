using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class OrganizationPositionConfiguration : IEntityTypeConfiguration<OrganizationPosition>
{
    public void Configure(EntityTypeBuilder<OrganizationPosition> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Users)
            .WithOne(x => x.OrganizationPosition)
            .HasForeignKey(x => x.OrganizationPositionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
