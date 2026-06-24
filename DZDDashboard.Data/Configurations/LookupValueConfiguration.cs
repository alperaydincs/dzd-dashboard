using DZDDashboard.Common.Constants;
using DZDDashboard.Common.Validation;
using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class LookupValueConfiguration : IEntityTypeConfiguration<LookupValue>
{
    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<LookupValue> builder)
    {
        builder.ToTable("LookupValues");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Category).IsRequired().HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(x => x.Value).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasIndex(x => new { x.Category, x.Value }).IsUnique();

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(BuildSeed());
    }

    private static IEnumerable<LookupValue> BuildSeed()
    {
        var id = 1;
        var rows = new List<LookupValue>();
        foreach (var category in LookupCategories.All)
        {
            var seq = 1;
            foreach (var value in LookupCategories.DefaultsFor(category))
                rows.Add(new LookupValue
                {
                    Id        = id++,
                    Category  = category,
                    Value     = value,
                    Sequence  = seq++,
                    CreatedAt = SeedTimestamp
                });
        }
        return rows;
    }
}
