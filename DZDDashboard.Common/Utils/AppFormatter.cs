using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

    public static string FormatDate(DateTime? dt, string format = "MMMM dd, yyyy")
    {
        return dt is null ? "-" : dt.Value.ToString(format);
    }

    public static string? FormatDurationFrom(DateTime? start)
    {
        if (start is null) return null;
        var now = DateTime.UtcNow.Date;
        var totalMonths = (now.Year - start.Value.Year) * 12 + (now.Month - start.Value.Month);
        if (now.Day < start.Value.Day) totalMonths = Math.Max(0, totalMonths - 1);
        var years = totalMonths / 12;
        var months = totalMonths % 12;
        if (years <= 0 && months <= 0) return "Less than a month";
        if (years <= 0) return $"{months} month{(months > 1 ? "s" : "")}";
        if (months <= 0) return $"{years} year{(years > 1 ? "s" : "")}";
        return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
    }

    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try { return new EmailAddressAttribute().IsValid(email); }
        catch { return false; }
    }

    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return true; // Optional
        return Regex.IsMatch(phone, @"^\+?[0-9]{6,20}$");
    }
}
