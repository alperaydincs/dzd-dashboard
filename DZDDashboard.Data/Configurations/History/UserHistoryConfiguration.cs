using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class UserHistoryConfiguration : HistoryEntityConfigurationBase<UserHistory>
{
    protected override string TableName => "UserHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<UserHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
