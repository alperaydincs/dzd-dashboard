using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class CompanyHistoryConfiguration : HistoryEntityConfigurationBase<CompanyHistory>
{
    protected override string TableName => "CompanyHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<CompanyHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
