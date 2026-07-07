using DZDDashboard.Data.Entities.History;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations.History;

public class UserCvDocumentHistoryConfiguration : HistoryEntityConfigurationBase<UserCvDocumentHistory>
{
    protected override string TableName => "UserCvDocumentHistory";

    protected override void ConfigureHistory(EntityTypeBuilder<UserCvDocumentHistory> builder)
    {
        builder.HasIndex(x => x.Id);
    }
}
