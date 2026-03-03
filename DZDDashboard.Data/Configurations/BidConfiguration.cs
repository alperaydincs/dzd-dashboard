using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable("Bids");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.JiraProjectNo)
               .HasMaxLength(50);

        builder.Property(b => b.ProjectName)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(b => b.TshirtSize)
               .HasMaxLength(10);

        builder.Property(b => b.Budget)
               .HasColumnType("decimal(18, 2)");

        builder.Property(b => b.Notes)
               .HasMaxLength(1000);

        builder.HasIndex(b => b.JiraProjectNo).IsUnique();

        builder.HasOne(b => b.Bank)
               .WithMany()
               .HasForeignKey(b => b.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.DzdStatus)
               .WithMany()
               .HasForeignKey(b => b.DzdStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Department)
               .WithMany()
               .HasForeignKey(b => b.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Team)
               .WithMany()
               .HasForeignKey(b => b.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Period)
               .WithMany()
               .HasForeignKey(b => b.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.ProjectManager)
               .WithMany()
               .HasForeignKey(b => b.ProjectManagerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Analyst)
               .WithMany()
               .HasForeignKey(b => b.AnalystId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Developer)
               .WithMany()
               .HasForeignKey(b => b.DeveloperId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.IntertechTeam)
               .WithMany()
               .HasForeignKey(b => b.IntertechTeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.ModifiedBy)
               .WithMany()
               .HasForeignKey(b => b.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}