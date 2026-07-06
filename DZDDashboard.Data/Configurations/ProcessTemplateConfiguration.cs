using DZDDashboard.Common.Constants;
using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class ProcessTemplateConfiguration : IEntityTypeConfiguration<ProcessTemplate>
{
    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public const int GeneralOnboardingId = 1;
    public const int ResignationId       = 2;
    public const int TerminationId       = 3;

    public void Configure(EntityTypeBuilder<ProcessTemplate> builder)
    {
        builder.ToTable("ProcessTemplates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Kind).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasIndex(x => x.Kind);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new ProcessTemplate { Id = GeneralOnboardingId, Kind = ProcessTemplateKinds.Onboarding, Name = "General Onboarding", Sequence = 1, CreatedAt = SeedTimestamp },
            new ProcessTemplate { Id = ResignationId, Kind = ProcessTemplateKinds.Offboarding, Name = "Resignation", Sequence = 1, CreatedAt = SeedTimestamp },
            new ProcessTemplate { Id = TerminationId, Kind = ProcessTemplateKinds.Offboarding, Name = "Termination", Sequence = 2, CreatedAt = SeedTimestamp }
        );
    }
}
