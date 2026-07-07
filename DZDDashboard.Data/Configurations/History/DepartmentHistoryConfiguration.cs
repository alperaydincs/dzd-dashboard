using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class DepartmentHistoryConfiguration : HistoryEntityConfigurationBase<DepartmentHistory>
{
    protected override string TableName => "DepartmentHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<DepartmentHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
