using DZDDashboard.Client.Localization;
using DZDDashboard.Client.Theme;
using DZDDashboard.Common.Constants;
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

public sealed record GradeBenefit(
    string Icon,
    string IconBg,
    string IconColor,
    string Name,
    string Value);

public static class GradeProgressCalculator
{
    public static List<CareerPathRuleDto> PathRulesOrdered(CareerPathDto? path)
        => path?.Rules.OrderBy(r => r.Grade).ToList() ?? [];

    public static CareerPathRuleDto? NextGradeRule(CareerPathDto? path, int? grade)
    {
        var rules = PathRulesOrdered(path);
        return grade is null ? rules.FirstOrDefault() : rules.FirstOrDefault(r => r.Grade > grade);
    }

    public static CareerPathRuleDto? CurrentGradeRule(CareerPathDto? path, int? grade)
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

    public static string FormatRoleDuration(AppLocalizer loc, RoleDurationDto d)
        => FormatDurationLocalized(loc, d.Years.GetValueOrDefault(), d.Months.GetValueOrDefault());

    private static string FormatDurationLocalized(AppLocalizer loc, int years, int months)
    {
        if (years <= 0 && months <= 0) return loc["duration.lessThanMonth"];

        var yearsPart  = years  > 0 ? string.Format(years  == 1 ? loc["duration.yearSingular"]  : loc["duration.yearPlural"],  years)  : null;
        var monthsPart = months > 0 ? string.Format(months == 1 ? loc["duration.monthSingular"] : loc["duration.monthPlural"], months) : null;
        return string.Join(" ", new[] { yearsPart, monthsPart }.Where(s => s is not null));
    }

    private static string FormatElapsedSince(AppLocalizer loc, DateTime? start)
    {
        if (start is null) return "-";
        var (years, months) = AppFormatter.GetElapsedYearsMonths(start);
        return FormatDurationLocalized(loc, years, months);
    }

    public static List<GradeRequirement> NextGradeRequirements(
        AppLocalizer loc, CareerPathDto? path, int? grade, DateTime? userStartDate, DateTime? positionStartDate)
    {
        if (CurrentGradeRule(path, grade) is not { } next) return [];

        var rows = new List<GradeRequirement>();
        var required     = loc["employeeProfile.requiredValue"];
        var pendingReview = loc["employeeProfile.pendingReview"];

        GradeRequirement Duration(string icon, string bg, string fg, string name,
            RoleDurationDto reqDuration, int actualMonths, string currentStatus)
        {
            var requiredMonths = ToMonths(reqDuration);
            var progress = requiredMonths <= 0 ? 100 : Math.Clamp(actualMonths * 100 / requiredMonths, 0, 100);
            var status = progress >= 100 ? ReqStatus.Completed : actualMonths > 0 ? ReqStatus.InProgress : ReqStatus.Pending;
            return new(icon, bg, fg, name, FormatRoleDuration(loc, reqDuration), currentStatus, progress, status);
        }

        if (ToMonths(next.MinExperience) > 0)
            rows.Add(Duration(DzdIcons.Clock, "rgba(43, 56, 245, 0.1)", "#2B38F5", loc["employeeProfile.reqMinExperience"],
                next.MinExperience, MonthsSince(userStartDate),
                FormatElapsedSince(loc, userStartDate)));

        if (ToMonths(next.MinRoleTime) > 0)
            rows.Add(Duration(DzdIcons.CalendarCheck, "rgba(244, 96, 54, 0.1)", "#F46036", loc["employeeProfile.reqTimeInRole"],
                next.MinRoleTime, MonthsSince(positionStartDate),
                FormatElapsedSince(loc, positionStartDate)));

        if (next.ProjectObjective is int po and > 0)
            rows.Add(new(DzdIcons.Target, "rgba(112, 163, 127, 0.15)", "#70A37F", loc["employeeProfile.reqProjectGoal"],
                string.Format(loc["employeeProfile.projectCount"], po), pendingReview, 0, ReqStatus.Pending));

        if (next.ManagerPerformanceEvaluation)
            rows.Add(new(DzdIcons.UserRound, "rgba(254, 168, 47, 0.15)", "#FEA82F", loc["employeeProfile.reqManagerRating"],
                required, pendingReview, 0, ReqStatus.Pending));

        if (next.TechnicalInterview)
            rows.Add(new(DzdIcons.ShieldCheck, "rgba(43, 56, 245, 0.1)", "#2B38F5", loc["employeeProfile.reqTechnicalInterview"],
                required, pendingReview, 0, ReqStatus.Pending));

        if (next.CaseStudy)
            rows.Add(new(DzdIcons.FileText, "rgba(43, 56, 245, 0.1)", "#2B38F5", loc["employeeProfile.reqCaseStudy"],
                required, pendingReview, 0, ReqStatus.Pending));

        if (next.EnglishProficiency)
            rows.Add(new(DzdIcons.Languages, "rgba(112, 163, 127, 0.15)", "#70A37F", loc["employeeProfile.reqEnglishProficiency"],
                required, pendingReview, 0, ReqStatus.Pending));

        if (next.AssessmentCenterApplication)
            rows.Add(new(DzdIcons.Target, "rgba(254, 168, 47, 0.15)", "#FEA82F", loc["employeeProfile.reqAssessmentCenter"],
                required, pendingReview, 0, ReqStatus.Pending));

        if (next.CommitteeApproval)
            rows.Add(new(DzdIcons.Users, "rgba(244, 96, 54, 0.1)", "#F46036", loc["employeeProfile.reqCommitteeApproval"],
                required, pendingReview, 0, ReqStatus.Pending));

        return rows;
    }

    public static List<GradeBenefit> CurrentGradeBenefits(AppLocalizer loc, CareerPathDto? path, int? grade)
        => CurrentGradeRule(path, grade) is { } rule ? BuildBenefits(loc, rule) : [];

    private static List<GradeBenefit> BuildBenefits(AppLocalizer loc, CareerPathRuleDto rule)
    {
        var items = new List<GradeBenefit>();

        if (rule.SalaryIncreasePercent is { } pct and > 0)
            items.Add(new(DzdIcons.TrendingUp, "rgba(112, 163, 127, 0.15)", "#70A37F",
                loc["employeeProfile.benefitSalaryIncrease"], $"{pct:0.##}%"));

        if (rule.PrivatePensionInsuranceAmount is { } pension and > 0)
            items.Add(new(DzdIcons.PiggyBank, "rgba(43, 56, 245, 0.1)", "#2B38F5",
                loc["employeeProfile.benefitPrivatePension"], FormatMoney(pension, rule.PrivatePensionInsuranceCurrency)));

        if (rule.EmployerContributionUpperLimitAmount is { } contribution and > 0)
            items.Add(new(DzdIcons.Landmark, "rgba(254, 168, 47, 0.15)", "#FEA82F",
                loc["employeeProfile.benefitEmployerContribution"], FormatMoney(contribution, rule.EmployerContributionUpperLimitCurrency)));

        if (rule.MealAllowanceAmount is { } meal and > 0)
            items.Add(new(DzdIcons.Utensils, "rgba(244, 96, 54, 0.1)", "#F46036",
                loc["employeeProfile.benefitMealAllowance"], FormatMoney(meal, rule.MealAllowanceCurrency)));

        return items;
    }

    private static string FormatMoney(decimal amount, string? currency)
        => $"{Currencies.Symbol(currency ?? Currencies.Try)}{amount:#,##0.##}";
}
