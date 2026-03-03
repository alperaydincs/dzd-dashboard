using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroups");

        builder.HasKey(ug => ug.Id);

        builder.Property(ug => ug.GroupName)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(ug => ug.GroupName).IsUnique();

        builder.HasMany(ug => ug.User)
               .WithOne(u => u.UserGroup)
               .HasForeignKey(u => u.UserGroupId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ug => ug.ModifiedBy)
               .WithMany()
               .HasForeignKey(ug => ug.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}