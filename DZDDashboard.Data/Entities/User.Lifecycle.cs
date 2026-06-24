using DZDDashboard.Common.Constants;

namespace DZDDashboard.Data.Entities;

public partial class User
{
    public string LifecycleStatus { get; set; } = UserLifecycleStatuses.Active;

    public OnboardingProcess?  OnboardingProcess  { get; set; }
    public OffboardingProcess? OffboardingProcess { get; set; }
}
