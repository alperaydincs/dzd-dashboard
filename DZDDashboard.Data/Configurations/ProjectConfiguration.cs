using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.JiraProjectNo)
               .HasMaxLength(50);

        builder.Property(p => p.JiraTaskNo)
               .HasMaxLength(50);

        builder.Property(p => p.ProjectName)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(p => p.TotalEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.DeveloperEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.AnalystEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.ProjectManagerEffort)
               .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Notes)
               .HasMaxLength(1000);

        builder.Property(p => p.Color)
               .HasMaxLength(20);

        builder.Property(p => p.IsIncludedInBonus)
               .HasDefaultValue(true);

        builder.HasIndex(p => new { p.JiraProjectNo, p.JiraTaskNo }).IsUnique();

        builder.HasOne(p => p.Bank)
               .WithMany(b => b.Projects)
               .HasForeignKey(p => p.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.DzdStatus)
               .WithMany(d => d.Projects)
               .HasForeignKey(p => p.DzdStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.JiraStatus)
               .WithMany(j => j.Projects)
               .HasForeignKey(p => p.JiraStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Department)
               .WithMany(d => d.Project)
               .HasForeignKey(p => p.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Team)
               .WithMany(t => t.Projects)
               .HasForeignKey(p => p.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Developer)
               .WithMany()
               .HasForeignKey(p => p.DeveloperId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Analyst)
               .WithMany()
               .HasForeignKey(p => p.AnalystId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ProjectManager)
               .WithMany()
               .HasForeignKey(p => p.ProjectManagerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Period)
               .WithMany(pe => pe.Projects)
               .HasForeignKey(p => p.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.IntertechTeam)
               .WithMany(it => it.Projects)
               .HasForeignKey(p => p.IntertechTeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.ModifiedBy)
               .WithMany()
               .HasForeignKey(p => p.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}