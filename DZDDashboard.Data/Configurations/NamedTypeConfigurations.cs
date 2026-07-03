using DZDDashboard.Common.Constants;
using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

internal static class NamedTypeConfig
{
    public static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Apply<T>(EntityTypeBuilder<T> builder, string table, IReadOnlyList<string> seedNames)
        where T : NamedTypeEntity, new()
    {
        builder.ToTable(table);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        var id = 1;
        builder.HasData(seedNames.Select(name => new T { Id = id++, Name = name, CreatedAt = SeedTimestamp }));
    }
}

public class AdditionalPaymentTypeConfiguration : IEntityTypeConfiguration<AdditionalPaymentTypeEntity>
{
    public void Configure(EntityTypeBuilder<AdditionalPaymentTypeEntity> b)
        => NamedTypeConfig.Apply(b, "AdditionalPaymentTypes", AdditionalPaymentTypes.All);
}

public class DeductionTypeConfiguration : IEntityTypeConfiguration<DeductionTypeEntity>
{
    public void Configure(EntityTypeBuilder<DeductionTypeEntity> b)
        => NamedTypeConfig.Apply(b, "DeductionTypes", DeductionTypes.All);
}

public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractTypeEntity>
{
    public void Configure(EntityTypeBuilder<ContractTypeEntity> b)
        => NamedTypeConfig.Apply(b, "ContractTypes", ContractTypes.All);
}

public class WorkModelConfiguration : IEntityTypeConfiguration<WorkModelEntity>
{
    public void Configure(EntityTypeBuilder<WorkModelEntity> b)
        => NamedTypeConfig.Apply(b, "WorkModels", WorkModels.All);
}

public class EducationLevelConfiguration : IEntityTypeConfiguration<EducationLevelEntity>
{
    public void Configure(EntityTypeBuilder<EducationLevelEntity> b)
        => NamedTypeConfig.Apply(b, "EducationLevels", EducationLevels.All);
}

public class DependentTypeConfiguration : IEntityTypeConfiguration<DependentTypeEntity>
{
    public void Configure(EntityTypeBuilder<DependentTypeEntity> b)
        => NamedTypeConfig.Apply(b, "DependentTypes", DependentTypes.All);
}
