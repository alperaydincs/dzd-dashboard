using DZDDashboard.Client.Components.Pages.Admin.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Admin;

/// <summary>Code-behind for GradeManagementPanel.razor — separated from UI markup (SRP).</summary>
public partial class GradeManagementPanel
{
    [Inject] private IDialogService            DialogServiceRef { get; set; } = default!;
    [Inject] private ISnackbar                 Snackbar         { get; set; } = default!;
    [Inject] private IOrganizationClientService OrgService      { get; set; } = default!;

    private bool                  _loading     = true;
    private string?               _loadError;
    private List<CareerPathDto>   _careerPaths = [];
    private List<JobDto>          _allJobs     = [];
    private List<UserGroupDto>    _userGroups  = [];
    private HashSet<int>          _expandedIds = [];

    private static readonly DialogOptions SmallDialog  = new() { MaxWidth = MaxWidth.Small,  FullWidth = true };
    private static readonly DialogOptions MediumDialog = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };

    protected override async Task OnInitializedAsync() => await LoadData();

    private async Task LoadData()
    {
        _loading   = true;
        _loadError = null;
        try
        {
            var t1   = OrgService.GetCareerPathsAsync();
            var t2   = OrgService.GetJobsAsync();
            var t3   = OrgService.GetUserGroupsAsync();
            await Task.WhenAll(t1, t2, t3);
            _careerPaths = t1.Result;
            _allJobs     = t2.Result;
            _userGroups  = t3.Result;
        }
        catch (Exception)
        {
            _loadError = "Failed to load career paths. Please refresh the page.";
            Snackbar.Add(_loadError, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void TogglePath(int id) { if (!_expandedIds.Add(id)) _expandedIds.Remove(id); }

    // ── Career Path CRUD ──────────────────────────────────────────────────────

    private async Task OpenAddCareerPathDialog()
    {
        if (_userGroups.Count == 0)
        {
            Snackbar.Add("No employee groups found. Create one in Settings → Organization first.", Severity.Warning);
            return;
        }

        var dialog = await DialogServiceRef.ShowAsync<CareerPathDialog>("Add Career Path",
            new() { ["UserGroups"] = _userGroups }, SmallDialog);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: CareerPathDto dto })
        {
            var resp = await OrgService.CreateCareerPathAsync(dto);
            if (resp.IsSuccessStatusCode)
            {
                await LoadData();
                var created = _careerPaths.FirstOrDefault(p => p.Name == dto.Name && p.UserGroupId == dto.UserGroupId);
                if (created != null) _expandedIds.Add(created.Id);
                Snackbar.Add("Career path created.", Severity.Success);
            }
            else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to create career path.", Severity.Error);
        }
    }

    private async Task OpenEditCareerPathDialog(CareerPathDto path)
    {
        var dialog = await DialogServiceRef.ShowAsync<CareerPathDialog>("Edit Career Path",
            new() { ["ExistingPath"] = path, ["UserGroups"] = _userGroups }, SmallDialog);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: CareerPathDto updated })
        {
            var resp = await OrgService.UpdateCareerPathAsync(updated);
            if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add("Career path updated.", Severity.Success); }
            else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update career path.", Severity.Error);
        }
    }

    private async Task DeleteCareerPath(CareerPathDto path)
    {
        if (await DialogServiceRef.ShowMessageBox("Delete Career Path",
            $"Delete \"{path.Name}\" and all its grade levels?",
            yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await OrgService.DeleteCareerPathAsync(path.Id);
        if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add("Career path deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete career path.", Severity.Error);
    }

    // ── Grade Level CRUD ──────────────────────────────────────────────────────

    private async Task OpenAddRuleDialog(CareerPathDto path)
    {
        var nextGrade = path.Rules.Count > 0 ? path.Rules.Max(r => r.Grade) + 1 : 1;

        var dialog = await DialogServiceRef.ShowAsync<GradeLevelDialog>("Add Grade Level",
            new() { ["Rule"] = new CareerMapRuleDto { CareerPathId = path.Id, Grade = nextGrade },
                    ["AvailableJobs"] = _allJobs }, MediumDialog);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: CareerMapRuleDto rule })
        {
            rule.CareerPathId = path.Id;
            var resp = await OrgService.CreateCareerMapRuleAsync(rule);
            if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add("Grade level added.", Severity.Success); }
            else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to add grade level.", Severity.Error);
        }
    }

    private async Task OpenEditRuleDialog(CareerPathDto path, CareerMapRuleDto rule)
    {
        var clone = rule with { PositionJobIds = rule.PositionJobIds.ToList(), PositionJobs = rule.PositionJobs.ToList() };

        var dialog = await DialogServiceRef.ShowAsync<GradeLevelDialog>(
            $"Edit Grade — {path.Name} G{rule.Grade}",
            new() { ["Rule"] = clone, ["AvailableJobs"] = _allJobs }, MediumDialog);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: CareerMapRuleDto updated })
        {
            var resp = await OrgService.UpdateCareerMapRuleAsync(updated);
            if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add("Grade level updated.", Severity.Success); }
            else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update grade level.", Severity.Error);
        }
    }

    private async Task DeleteRule(CareerMapRuleDto rule)
    {
        if (await DialogServiceRef.ShowMessageBox("Delete Grade Level",
            $"Delete G{rule.Grade}?", yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await OrgService.DeleteCareerMapRuleAsync(rule.Id);
        if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add("Grade level deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete grade level.", Severity.Error);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string FormatMinExperience(CareerMapRuleDto r) =>
        FormatDuration(r.MinExperience);

    private static string FormatRetention(CareerMapRuleDto r) =>
        FormatDuration(r.MinRoleTime);

    private static string FormatDuration(RoleDurationDto? duration)
    {
        if (duration is null) return "—";
        var years  = duration.Years  is > 0 ? $"{duration.Years}yr"  : null;
        var months = duration.Months is > 0 ? $"{duration.Months}mo" : null;
        return (years, months) switch
        {
            (not null, not null) => $"{years} {months}",
            (not null, null)     => years,
            (null,     not null) => months,
            _                    => "—"
        };
    }
}
