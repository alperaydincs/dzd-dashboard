using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class StoredFileHistoryConfiguration : HistoryEntityConfigurationBase<StoredFileHistory>
{
    protected override string TableName => "StoredFileHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<StoredFileHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
