using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class IssuePriorityConfiguration : IEntityTypeConfiguration<IssuePriority>
{
    public void Configure(EntityTypeBuilder<IssuePriority> builder)
    {
        builder.ToTable("IssuePriorities");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.PriorityName)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxNameLength);

        builder.HasIndex(i => i.PriorityName).IsUnique();

        builder.HasMany(i => i.Itsms)
               .WithOne(it => it.IssuePriority)
               .HasForeignKey(it => it.IssuePriorityId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ModifiedBy)
               .WithMany()
               .HasForeignKey(i => i.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
