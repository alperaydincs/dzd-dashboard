using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DZDDashboard.Common.Validation;

namespace DZDDashboard.Common.Utils;

public static class AppFormatter
{
    public static string BuildFullName(string? first, string? last)
    {
        return string.Join(" ", new[] { first?.Trim(), last?.Trim() }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }

    public static string GetInitials(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2) return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        return name.Length > 0 ? $"{name[0]}".ToUpperInvariant() : "?";
    }

    public static string FormatDate(DateTime? dt, string format = "MMM d, yyyy")
    {
        return dt is null ? "-" : dt.Value.ToString(format, CultureInfo.CurrentCulture);
    }

    public static string FormatDateLocal(DateTime? dt, string format = "MMM d, yyyy")
    {
        if (dt is null) return "-";
        var utc = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
        return utc.ToLocalTime().ToString(format, CultureInfo.CurrentCulture);
    }

    public static (int Years, int Months) GetElapsedYearsMonths(DateTime? start)
    {
        if (start is null) return (0, 0);
        var now = DateTime.UtcNow.Date;
        var totalMonths = (now.Year - start.Value.Year) * 12 + (now.Month - start.Value.Month);
        if (now.Day < start.Value.Day) totalMonths = Math.Max(0, totalMonths - 1);
        return (totalMonths / 12, totalMonths % 12);
    }

    public static string? FormatDurationFrom(DateTime? start)
    {
        if (start is null) return null;
        var (years, months) = GetElapsedYearsMonths(start);
        if (years <= 0 && months <= 0) return "Less than a month";
        if (years <= 0) return $"{months} month{(months > 1 ? "s" : "")}";
        if (months <= 0) return $"{years} year{(years > 1 ? "s" : "")}";
        return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
    }

    /// <summary>
    /// Formats a phone number for display by grouping digits (e.g. "+905434762342" -&gt; "+90 543 476 2342").
    /// Assumes the last 10 digits are the national number and any digits before that are the country code
    /// (matches typical mobile numbers such as Turkish "+90 5XX XXX XXXX"); this is a display-only heuristic,
    /// not a validated parse.
    /// </summary>
    public static string FormatPhoneDisplay(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
        var hasPlus = phone.TrimStart().StartsWith('+');
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        if (digits.Length == 0) return phone;

        var countryCode = string.Empty;
        var national    = digits;
        if (hasPlus && digits.Length > 10)
        {
            countryCode = digits[..^10];
            national    = digits[^10..];
        }

        var groups = new List<string>();
        var i = 0;
        while (i < national.Length)
        {
            var remaining = national.Length - i;
            var take = remaining == 4 ? 4 : Math.Min(3, remaining);
            if (remaining - take == 1) take++;
            groups.Add(national.Substring(i, take));
            i += take;
        }

        var formattedNational = string.Join(' ', groups);
        return countryCode.Length > 0 ? $"+{countryCode} {formattedNational}"
             : hasPlus ? $"+{formattedNational}"
             : formattedNational;
    }

    private static readonly EmailAddressAttribute _emailValidator = new();

    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return _emailValidator.IsValid(email);
    }

    public static bool IsValidPhone(string? phone)
        => string.IsNullOrWhiteSpace(phone) || PhoneValidator.IsValid(phone);

    public static string FormatElapsed(DateTime utcTime)
    {
        var elapsed = DateTime.UtcNow - utcTime;
        return elapsed.TotalMinutes < 1  ? "just now"
             : elapsed.TotalHours   < 1  ? $"{(int)elapsed.TotalMinutes}m ago"
             : elapsed.TotalDays    < 1  ? $"{(int)elapsed.TotalHours}h ago"
             : $"{(int)elapsed.TotalDays}d ago";
    }
}
