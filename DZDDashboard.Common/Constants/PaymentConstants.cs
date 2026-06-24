namespace DZDDashboard.Common.Constants;

public static class Currencies
{
    public const string Try = "TRY";
    public const string Usd = "USD";
    public const string Eur = "EUR";

    public static readonly IReadOnlyList<string> All = [Try, Usd, Eur];

    public static string Display(string code) => code switch
    {
        Try => "Turkish Lira (TRY)",
        Usd => "US Dollar (USD)",
        Eur => "Euro (EUR)",
        _   => code
    };
}

public static class PaymentPeriods
{
    public const string Monthly = "Monthly";
    public const string Weekly  = "Weekly";
    public const string Hourly  = "Hourly";
    public const string Yearly  = "Yearly";

    public static readonly IReadOnlyList<string> All = [Monthly, Weekly, Hourly, Yearly];
}

public static class PayTypes
{
    public const string Gross = "Gross";
    public const string Net   = "Net";

    public static readonly IReadOnlyList<string> All = [Gross, Net];
}

public static class BenefitTypes
{
    public const string PrivateHealthInsurance = "Health Insurance";    public const string PrivatePension         = "Private Pension";    public const string Other                  = "Other";

    public static readonly IReadOnlyList<string> All =
        [PrivateHealthInsurance, PrivatePension, Other];
}

public static class BenefitPayers
{
    public const string Employer = "Employer";
    public const string Employee = "Employee";

    public static readonly IReadOnlyList<string> All = [Employer, Employee];
}

public static class PaymentSources
{
    public const string Manual     = "Manual";
    public const string Onboarding = "Onboarding";

    public static readonly IReadOnlyList<string> All = [Manual, Onboarding];
}

public static class DependentTypes
{
    public const string Spouse = "Spouse";
    public const string Child  = "Child";
    public const string Other  = "Other";

    public static readonly IReadOnlyList<string> All = [Spouse, Child, Other];
}

public static class AdditionalPaymentTypes
{
    public const string Premium  = "Premium";
    public const string Bonus    = "Bonus";
    public const string Advance  = "Advance";
    public const string Overtime = "Overtime";
    public const string Other    = "Other";

    public static readonly IReadOnlyList<string> All = [Premium, Bonus, Advance, Overtime, Other];
}

public static class AdditionalPaymentPeriods
{
    public const string OneTime = "One-Time";
    public const string Monthly = "Monthly";
    public const string Weekly  = "Weekly";

    public static readonly IReadOnlyList<string> All = [OneTime, Monthly, Weekly];
}

public static class DeductionTypes
{
    public const string SocialSecurity = "Social Security";    public const string Garnishment    = "Garnishment";    public const string Advance        = "Advance";    public const string Tax            = "Tax";
    public const string Other          = "Other";

    public static readonly IReadOnlyList<string> All =
        [SocialSecurity, Garnishment, Advance, Tax, Other];
}
