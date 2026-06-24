namespace DZDDashboard.Common.Constants;

public static class UserLifecycleStatuses
{
    public const string Candidate   = "Candidate";
    public const string Onboarding  = "Onboarding";
    public const string Active      = "Active";
    public const string Offboarding = "Offboarding";
    public const string Exited      = "Exited";

    public static readonly IReadOnlyList<string> All =
        [Candidate, Onboarding, Active, Offboarding, Exited];
}

public static class ProcessStatuses
{
    public const string InProgress = "InProgress";
    public const string Completed  = "Completed";
    public const string Cancelled  = "Cancelled";

    public static readonly IReadOnlyList<string> All = [InProgress, Completed, Cancelled];
}

public static class ChecklistItemStatuses
{
    public const string Pending   = "Pending";
    public const string Completed = "Completed";
    public const string Skipped   = "Skipped";

    public static readonly IReadOnlyList<string> All = [Pending, Completed, Skipped];
}

public static class DocumentReviewStatuses
{
    public const string Pending        = "Pending";
    public const string Approved       = "Approved";
    public const string NeedsCorrection = "NeedsCorrection";

    public static readonly IReadOnlyList<string> All = [Pending, Approved, NeedsCorrection];
}

public static class OffboardingTypes
{
    public const string Resignation = "Resignation";
    public const string Termination = "Termination";

    public static readonly IReadOnlyList<string> All = [Resignation, Termination];
}

public static class ChecklistBenefitKinds
{
    public const string None = "None";
    public const string Bes  = "BES";
    public const string Oss  = "OSS";
}

public static class TemplateProcessTypes
{
    public const string Onboarding             = "Onboarding";
    public const string OffboardingResignation = "Offboarding:Resignation";
    public const string OffboardingTermination = "Offboarding:Termination";

    public static readonly IReadOnlyList<string> All =
        [Onboarding, OffboardingResignation, OffboardingTermination];

    public static string ForOffboarding(string offboardingType) => offboardingType switch
    {
        OffboardingTypes.Termination => OffboardingTermination,
        _                            => OffboardingResignation
    };
}

public sealed record ChecklistStepDefinition(
    string Key,
    string Title,
    int Sequence,
    bool IsRequired = true,
    bool IsGate = false,
    string BenefitKind = ChecklistBenefitKinds.None,
    bool RequiresDocument = false);

public static class OnboardingStepCatalog
{
    public const string Documents      = "documents";
    public const string Contract       = "contract";
    public const string SgkEntry       = "sgk-entry";
    public const string AccountantInfo = "accountant-info";
    public const string Bes            = "bes";
    public const string Oss            = "oss";
    public const string AssetHandover  = "asset-handover";

    public static readonly IReadOnlyList<ChecklistStepDefinition> Steps =
    [
        new(Documents,      "Zorunlu evraklar tamamlandı",        1),
        new(Contract,       "Sözleşme hazırlandı ve imzalatıldı", 2),
        new(SgkEntry,       "SGK işe giriş yapıldı",              3),
        new(AccountantInfo, "Mali müşavire bilgi verildi",        4),
        new(Bes,            "BES açıldı (işveren + çalışan payı)", 5, BenefitKind: ChecklistBenefitKinds.Bes),
        new(Oss,            "ÖSS açıldı (çalışan + bağımlılar)",   6, BenefitKind: ChecklistBenefitKinds.Oss),
        new(AssetHandover,  "Bilgisayar teslim edildi (zimmet)",  7),
    ];
}

public static class OffboardingStepCatalog
{
    public static readonly IReadOnlyList<ChecklistStepDefinition> ResignationSteps =
    [
        new("resignation-letter", "İstifa alındı",        1),
        new("sgk-exit",           "SGK işten çıkış",      2),
        new("oss-cancel",         "ÖSS iptal",            3),
        new("bes-cancel",         "BES iptal",            4),
        new("access-revocation",  "Erişim kapatma",       5),
        new("asset-return",       "Zimmet iadesi",        6),
        new("final-settlement",   "Hakediş hesaplama",    7),
        new("payment-done",       "Ödeme yapıldı",        8, IsGate: true),
    ];

    public static readonly IReadOnlyList<ChecklistStepDefinition> TerminationSteps =
    [
        new("justification",      "Gerekçe dokümantasyonu",                1),
        new("mediator-meeting",   "Arabulucu/avukat görüşmesi (zorunlu)",  2, IsGate: true),
        new("settlement-calc",    "Hakediş/tazminat hesaplama",            3),
        new("approval",           "Onay",                                  4, IsRequired: false),
        new("sgk-exit",           "SGK işten çıkış",                       5),
        new("oss-bes-cancel",     "ÖSS/BES iptal",                         6),
        new("access-revocation",  "Erişim kapatma",                        7),
        new("asset-return",       "Zimmet iadesi",                         8),
        new("payment-done",       "Ödeme yapıldı",                         9, IsGate: true),
    ];

    public const string PaymentDoneKey = "payment-done";

    public static IReadOnlyList<ChecklistStepDefinition> For(string offboardingType) => offboardingType switch
    {
        OffboardingTypes.Termination => TerminationSteps,
        _                            => ResignationSteps,
    };
}
