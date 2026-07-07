using DZDDashboard.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

/// <summary>
/// Shared setup for every "*History" table: identity PK + common metadata columns
/// (Operation/HistoryRecordedAt/HistoryRecordedById). Subclasses only need to set the
/// table name, the index on the mirrored source-entity id, and any column-length overrides.
/// </summary>
public abstract class HistoryEntityConfigurationBase<THistory> : IEntityTypeConfiguration<THistory>
    where THistory : class, IHistoryEntity
{
    protected abstract string TableName { get; }

    public void Configure(EntityTypeBuilder<THistory> builder)
    {
        builder.ToTable(TableName);
        builder.HasKey(x => x.HistoryId);

        builder.Property(x => x.Operation).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Property(x => x.HistoryRecordedAt).IsRequired();

        ConfigureHistory(builder);
    }

    protected abstract void ConfigureHistory(EntityTypeBuilder<THistory> builder);
}
