using DZDDashboard.Client.Theme;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;

namespace DZDDashboard.Client.Services;

public enum ReqStatus { Completed, InProgress, Pending }

public sealed record GradeRequirement(
    string Icon,
    string IconBg,
    string IconColor,
    string Name,
    string Required,
    string CurrentStatus,
    int Progress,
    ReqStatus Status);

public static class GradeProgressCalculator
{
    public static List<CareerMapRuleDto> PathRulesOrdered(CareerPathDto? path)
        => path?.Rules.OrderBy(r => r.Grade).ToList() ?? [];

    public static CareerMapRuleDto? NextGradeRule(CareerPathDto? path, int? grade)
    {
        var rules = PathRulesOrdered(path);
        return grade is null ? rules.FirstOrDefault() : rules.FirstOrDefault(r => r.Grade > grade);
    }

    public static CareerMapRuleDto? CurrentGradeRule(CareerPathDto? path, int? grade)
    {
        var rules = PathRulesOrdered(path);
        return grade is null ? rules.FirstOrDefault() : rules.FirstOrDefault(r => r.Grade == grade);
    }

    public static int ToMonths(RoleDurationDto d) => d.Years.GetValueOrDefault() * 12 + d.Months.GetValueOrDefault();

    public static int MonthsSince(DateTime? start)
    {
        if (start is null) return 0;
        var now = DateTime.UtcNow.Date;
        var total = (now.Year - start.Value.Year) * 12 + (now.Month - start.Value.Month);
        if (now.Day < start.Value.Day) total--;
        return Math.Max(0, total);
    }

    public static string FormatRoleDuration(RoleDurationDto d)
        => d.Years is > 0 and var y ? $"{y} year{(y == 1 ? "" : "s")}"
         : d.Months is > 0 and var m ? $"{m} month{(m == 1 ? "" : "s")}"
         : "-";

    public static List<GradeRequirement> NextGradeRequirements(
        CareerPathDto? path, int? grade, DateTime? userStartDate, DateTime? positionStartDate)
    {
        if (CurrentGradeRule(path, grade) is not { } next) return [];

        var rows = new List<GradeRequirement>();

        static GradeRequirement Duration(string icon, string bg, string fg, string name,
            RoleDurationDto required, int actualMonths, string currentStatus)
        {
            var requiredMonths = ToMonths(required);
            var progress = requiredMonths <= 0 ? 100 : Math.Clamp(actualMonths * 100 / requiredMonths, 0, 100);
            var status = progress >= 100 ? ReqStatus.Completed : actualMonths > 0 ? ReqStatus.InProgress : ReqStatus.Pending;
            return new(icon, bg, fg, name, FormatRoleDuration(required), currentStatus, progress, status);
        }

        if (ToMonths(next.MinExperience) > 0)
            rows.Add(Duration(DzdIcons.Clock, "#E8EBFF", "#2B38F5", "Minimum Experience",
                next.MinExperience, MonthsSince(userStartDate),
                AppFormatter.FormatDurationFrom(userStartDate) ?? "-"));

        if (ToMonths(next.MinRoleTime) > 0)
            rows.Add(Duration(DzdIcons.CalendarCheck, "#FFF1E8", "#F46036", "Time in Current Role",
                next.MinRoleTime, MonthsSince(positionStartDate),
                AppFormatter.FormatDurationFrom(positionStartDate) ?? "-"));

        if (next.ProjectObjective is int po and > 0)
            rows.Add(new(DzdIcons.Target, "#E3F0EE", "#70A37F", "Project Goal Achievement",
                $"{po} project{(po == 1 ? "" : "s")}", "Pending review", 0, ReqStatus.Pending));

        if (next.ManagerPerformanceEvaluation)
            rows.Add(new(DzdIcons.UserRound, "#FFF8E8", "#FEA82F", "Manager Performance Rating",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.TechnicalInterview)
            rows.Add(new(DzdIcons.ShieldCheck, "#E8EBFF", "#2B38F5", "Technical Interview",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.CaseStudy)
            rows.Add(new(DzdIcons.FileText, "#E8EBFF", "#2B38F5", "Case Study",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.EnglishProficiency)
            rows.Add(new(DzdIcons.Languages, "#E3F0EE", "#70A37F", "English Proficiency",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.AssessmentCenterApplication)
            rows.Add(new(DzdIcons.Target, "#FFF8E8", "#FEA82F", "Assessment Center",
                "Required", "Pending review", 0, ReqStatus.Pending));

        if (next.CommitteeApproval)
            rows.Add(new(DzdIcons.Users, "#FFF1E8", "#F46036", "Committee Approval",
                "Required", "Pending review", 0, ReqStatus.Pending));

        return rows;
    }
}
