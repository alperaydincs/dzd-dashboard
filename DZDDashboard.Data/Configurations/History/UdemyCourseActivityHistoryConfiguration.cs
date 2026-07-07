using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class UdemyCourseActivityHistoryConfiguration : HistoryEntityConfigurationBase<UdemyCourseActivityHistory>
{
    protected override string TableName => "UdemyCourseActivityHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<UdemyCourseActivityHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
