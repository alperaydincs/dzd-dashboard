using DZDDashboard.Common.Validation;
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
            .HasMaxLength(ValidationConstants.MaxPositionNameLength);

        // Index on ParentId: used by DeletePositionAsync (WHERE ParentId = @id) and child-loading queries
        builder.HasIndex(x => x.ParentId)
               .HasDatabaseName("IX_OrganizationPositions_ParentId");

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
