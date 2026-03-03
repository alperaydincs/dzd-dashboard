using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations
{
    public class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
    {
        public void Configure(EntityTypeBuilder<UserAvatar> builder)
        {
            builder.ToTable("UserAvatars");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ContentBase64)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(a => a.ContentType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(a => a.User)
                   .WithOne(u => u.Avatar) 
                   .HasForeignKey<UserAvatar>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.ModifiedBy)
                   .WithMany()
                   .HasForeignKey(a => a.ModifiedById)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.UserId).IsUnique();
        }
    }
}