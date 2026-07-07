using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class ChildInfoHistoryConfiguration : HistoryEntityConfigurationBase<ChildInfoHistory>
{
    protected override string TableName => "ChildInfoHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<ChildInfoHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
