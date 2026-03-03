using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class IssueStatusConfiguration : IEntityTypeConfiguration<IssueStatus>
{
    public void Configure(EntityTypeBuilder<IssueStatus> builder)
    {
        builder.ToTable("IssueStatuses");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.StatusName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(i => i.StatusName).IsUnique();

        builder.HasMany(i => i.Itsms)
               .WithOne(it => it.IssueStatus)
               .HasForeignKey(it => it.IssueStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ModifiedBy)
               .WithMany()
               .HasForeignKey(i => i.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}