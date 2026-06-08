namespace DZDDashboard.Common.Constants;

/// <summary>
/// Domain-level string constants for the Employee Profile → Payment screen
/// (Salary, Benefits, Additional Payments). Stored as strings in the database
/// but drawn from a fixed set — centralises all magic strings in one place,
/// mirroring the convention used by <c>ContractTypes</c>/<c>WorkModels</c>/etc.
/// </summary>
public static class Currencies
{
    public const string Try = "TRY";
    public const string Usd = "USD";
    public const string Eur = "EUR";

    public static readonly IReadOnlyList<string> All = [Try, Usd, Eur];
}

/// <summary>How a recurring monetary amount repeats.</summary>
public static class PaymentPeriods
{
    public const string Monthly = "Monthly";
    public const string Weekly  = "Weekly";
    public const string Hourly  = "Hourly";
    public const string Yearly  = "Yearly";

    public static readonly IReadOnlyList<string> All = [Monthly, Weekly, Hourly, Yearly];
}

/// <summary>Whether a salary amount is expressed gross or net of deductions.</summary>
public static class PayTypes
{
    public const string Gross = "Gross";
    public const string Net   = "Net";

    public static readonly IReadOnlyList<string> All = [Gross, Net];
}

/// <summary>Benefit (yan hak) categories shown in the Benefits tab.</summary>
public static class BenefitTypes
{
    public const string Meal                   = "Meal";
    public const string Transport               = "Transport";
    public const string PrivateHealthInsurance = "PrivateHealthInsurance"; // ÖSS
    public const string PrivatePension          = "PrivatePension";        // BES
    public const string Other                   = "Other";

    public static readonly IReadOnlyList<string> All =
        [Meal, Transport, PrivateHealthInsurance, PrivatePension, Other];
}

/// <summary>Who bears the cost of a benefit line item.</summary>
public static class BenefitPayers
{
    public const string Employer = "Employer";
    public const string Employee = "Employee";

    public static readonly IReadOnlyList<string> All = [Employer, Employee];
}

/// <summary>Where a benefit/payment record originated from.</summary>
public static class PaymentSources
{
    public const string Manual     = "Manual";
    public const string Onboarding = "Onboarding";

    public static readonly IReadOnlyList<string> All = [Manual, Onboarding];
}

/// <summary>Relationship of a dependent to the employee (used by ÖSS dependents).</summary>
public static class DependentTypes
{
    public const string Spouse = "Spouse";
    public const string Child  = "Child";
    public const string Other  = "Other";

    public static readonly IReadOnlyList<string> All = [Spouse, Child, Other];
}

/// <summary>Additional (one-off or periodic) payment categories.</summary>
public static class AdditionalPaymentTypes
{
    public const string Premium  = "Premium";
    public const string Bonus    = "Bonus";
    public const string Advance  = "Advance";
    public const string Overtime = "Overtime";
    public const string Other    = "Other";

    public static readonly IReadOnlyList<string> All = [Premium, Bonus, Advance, Overtime, Other];
}

/// <summary>How an additional payment recurs.</summary>
public static class AdditionalPaymentPeriods
{
    public const string OneTime = "OneTime";
    public const string Monthly = "Monthly";
    public const string Weekly  = "Weekly";

    public static readonly IReadOnlyList<string> All = [OneTime, Monthly, Weekly];
}
