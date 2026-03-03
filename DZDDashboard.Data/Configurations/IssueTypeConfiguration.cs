using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class IssueTypeConfiguration : IEntityTypeConfiguration<IssueType>
{
    public void Configure(EntityTypeBuilder<IssueType> builder)
    {
        builder.ToTable("IssueTypes");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.TypeName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(i => i.TypeName).IsUnique();

        builder.HasMany(i => i.Itsms)
               .WithOne(it => it.IssueType)
               .HasForeignKey(it => it.IssueTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ModifiedBy)
               .WithMany()
               .HasForeignKey(i => i.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}