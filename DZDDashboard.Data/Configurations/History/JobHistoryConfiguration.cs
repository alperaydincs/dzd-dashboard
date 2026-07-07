using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class JobHistoryConfiguration : HistoryEntityConfigurationBase<JobHistory>
{
    protected override string TableName => "JobHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<JobHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
