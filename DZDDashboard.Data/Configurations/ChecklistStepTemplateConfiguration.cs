using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ChecklistStepTemplateConfiguration : IEntityTypeConfiguration<ChecklistStepTemplate>
{
    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<ChecklistStepTemplate> builder)
    {
        builder.ToTable("ChecklistStepTemplates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasIndex(x => x.ProcessTemplateId);

        builder.HasOne(x => x.ProcessTemplate)
            .WithMany(t => t.ChecklistItems)
            .HasForeignKey(x => x.ProcessTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(BuildSeed());
    }

    private static IEnumerable<ChecklistStepTemplate> BuildSeed()
    {
        var id = 1;
        var rows = new List<ChecklistStepTemplate>();

        void Add(int processTemplateId, IReadOnlyList<string> titles)
        {
            var sequence = 1;
            foreach (var title in titles)
                rows.Add(new ChecklistStepTemplate
                {
                    Id                = id++,
                    ProcessTemplateId = processTemplateId,
                    Title             = title,
                    Sequence          = sequence++,
                    IsRequired        = true,
                    CreatedAt         = SeedTimestamp
                });
        }

        Add(ProcessTemplateConfiguration.GeneralOnboardingId,
        [
            "Contract prepared and signed",
            "Social security registration completed",
            "Accountant notified",
            "Private Pension System (BES) account opened",
            "Private Health Insurance (ÖSS) opened",
            "Computer delivered"
        ]);
        Add(ProcessTemplateConfiguration.ResignationId,
        [
            "Resignation letter received",
            "Social security exit processed",
            "Access revoked",
            "Asset return confirmed",
            "Final settlement calculated"
        ]);
        Add(ProcessTemplateConfiguration.TerminationId,
        [
            "Justification documented",
            "Settlement/severance calculated",
            "Social security exit processed",
            "Access revoked",
            "Asset return confirmed"
        ]);

        return rows;
    }
}
