using DZDDashboard.Common.Validation;
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
               .HasMaxLength(ValidationConstants.MaxReferenceCodeLength);

        builder.Property(i => i.Active)
               .HasDefaultValue(true);

        builder.HasIndex(i => i.IssueKey).IsUnique();
        builder.HasIndex(i => i.AssigneeId).HasDatabaseName("IX_Itsms_AssigneeId");
        builder.HasIndex(i => i.PeriodId).HasDatabaseName("IX_Itsms_PeriodId");

        builder.HasOne(i => i.IssueType)
               .WithMany(it => it.Itsms)
               .HasForeignKey(i => i.IssueTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Bank)
               .WithMany(b => b.Itsms)
               .HasForeignKey(i => i.BankId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Assignee)
               .WithMany()
               .HasForeignKey(i => i.AssigneeId)
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
