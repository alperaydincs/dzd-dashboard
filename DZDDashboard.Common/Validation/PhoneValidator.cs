using System.Text.RegularExpressions;

namespace DZDDashboard.Common.Validation;

public static partial class PhoneValidator
{
    [GeneratedRegex(@"^\+?[0-9]{6,20}$")]
    private static partial Regex PhoneRegex();

    public static bool IsValid(string? phone)
        => !string.IsNullOrWhiteSpace(phone) && PhoneRegex().IsMatch(phone);
}
