namespace DZDDashboard.Services;

public class OnboardingOptions
{
    public const string SectionName = "Onboarding";

    public bool AutoStartEnabled { get; set; } = true;

    public string[] BypassEmails { get; set; } = [];
}
