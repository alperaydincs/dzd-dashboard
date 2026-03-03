using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ChildInfoConfiguration : IEntityTypeConfiguration<ChildInfo>
{
    public void Configure(EntityTypeBuilder<ChildInfo> builder)
    {
        builder.ToTable("UserChildren");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName).HasMaxLength(200);

        builder.HasOne(c => c.User)
               .WithMany(u => u.Children)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}