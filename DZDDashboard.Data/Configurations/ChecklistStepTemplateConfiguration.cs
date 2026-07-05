using DZDDashboard.Common.Constants;
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

        builder.Property(x => x.ProcessType).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.StepKey).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.Property(x => x.BenefitKind).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.HasIndex(x => new { x.ProcessType, x.StepKey }).IsUnique();

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

        void Add(string processType, IReadOnlyList<ChecklistStepDefinition> steps)
        {
            foreach (var step in steps)
                rows.Add(new ChecklistStepTemplate
                {
                    Id               = id++,
                    ProcessType      = processType,
                    StepKey          = step.Key,
                    Title            = step.Title,
                    Sequence         = step.Sequence,
                    IsRequired       = step.IsRequired,
                    IsGate           = step.IsGate,
                    RequiresDocument = step.RequiresDocument,
                    BenefitKind      = step.BenefitKind,
                    IsEnabled        = true,
                    CreatedAt        = SeedTimestamp
                });
        }

        Add(TemplateProcessTypes.Onboarding, OnboardingStepCatalog.Steps);
        Add(TemplateProcessTypes.OffboardingResignation, OffboardingStepCatalog.ResignationSteps);
        Add(TemplateProcessTypes.OffboardingTermination, OffboardingStepCatalog.TerminationSteps);

        return rows;
    }
}
