using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ItsmConfiguration : IEntityTypeConfiguration<Itsm>
{
    public void Configure(EntityTypeBuilder<Itsm> builder)
    {
        builder.ToTable("Itsms");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.IssueKey)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(i => i.Active)
               .HasDefaultValue(true);

        builder.HasIndex(i => i.IssueKey).IsUnique();

        builder.HasOne(i => i.IssueType)
               .WithMany(it => it.Itsms)
               .HasForeignKey(i => i.IssueTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Bank)
               .WithMany(b => b.Itsms)
               .HasForeignKey(i => i.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Asignee)
               .WithMany()
               .HasForeignKey(i => i.AsigneeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Team)
               .WithMany(t => t.Itsms)
               .HasForeignKey(i => i.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Resolution)
               .WithMany(r => r.Itsms)
               .HasForeignKey(i => i.ResolutionId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.IssuePriority)
               .WithMany(ip => ip.Itsms)
               .HasForeignKey(i => i.IssuePriorityId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.IssueStatus)
               .WithMany(it => it.Itsms)
               .HasForeignKey(i => i.IssueStatusId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ItsmPaymentType)
               .WithMany(ipt => ipt.Itsms)
               .HasForeignKey(i => i.ItsmPaymentTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Period)
               .WithMany()
               .HasForeignKey(i => i.PeriodId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.ModifiedBy)
               .WithMany()
               .HasForeignKey(i => i.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}