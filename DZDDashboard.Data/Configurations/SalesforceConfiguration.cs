using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class SalesforceConfiguration : IEntityTypeConfiguration<Salesforce>
{
    public void Configure(EntityTypeBuilder<Salesforce> builder)
    {
        builder.ToTable("Salesforces");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TaskTeam)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.Property(s => s.TaskPo)
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.Property(s => s.IsSuitable)
               .HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(s => s.Info)
               .HasMaxLength(ValidationConstants.MaxNotesLength);

        builder.HasIndex(s => new { s.TaskTeam, s.TaskPo }).IsUnique();

        builder.HasMany(s => s.Bids)
               .WithOne(b => b.IntertechTeam)
               .HasForeignKey(b => b.IntertechTeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Projects)
               .WithOne(p => p.IntertechTeam)
               .HasForeignKey(p => p.IntertechTeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.PayrollLocation)
               .WithMany(p => p.IntertechTeams)
               .HasForeignKey(s => s.PayrollLocationId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.ModifiedBy)
               .WithMany()
               .HasForeignKey(s => s.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
