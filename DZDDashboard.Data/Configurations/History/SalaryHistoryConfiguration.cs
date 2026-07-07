using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class SalaryHistoryConfiguration : HistoryEntityConfigurationBase<SalaryHistory>
{
    protected override string TableName => "SalaryHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<SalaryHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
