using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class EducationHistoryConfiguration : HistoryEntityConfigurationBase<EducationHistory>
{
    protected override string TableName => "EducationHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<EducationHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
