using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.HasIndex(t => t.Name).IsUnique();

        builder.HasOne(t => t.Department)
               .WithMany(d => d.Teams)
               .HasForeignKey(t => t.DepartmentId)
               .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.Users)
               .WithOne(u => u.Team)
               .HasForeignKey(u => u.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Projects)
               .WithOne(p => p.Team)
               .HasForeignKey(p => p.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Itsms)
               .WithOne(i => i.Team)
               .HasForeignKey(i => i.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.ModifiedBy)
               .WithMany()
               .HasForeignKey(t => t.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}