using DZDDashboard.Client.Components.Pages.Admin.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Admin;

public partial class GradeManagementPanel
{
    [Inject] private IDialogService            DialogServiceRef { get; set; } = default!;
    [Inject] private ISnackbar                 Snackbar         { get; set; } = default!;
    [Inject] private IOrganizationClientService OrgService      { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.AppLocalizer Loc          { get; set; } = default!;

    private bool                  _loading     = true;
    private string?               _loadError;
    private List<CareerPathDto>   _careerPaths = [];
    private List<JobDto>          _allJobs     = [];
    private List<UserGroupDto>    _userGroups  = [];
    private HashSet<int>          _expandedIds = [];

    private static readonly DialogOptions SmallDialog = new() { MaxWidth = MaxWidth.Small, FullWidth = true };

    private int?                  _editPathId;    private string                _editPathName = string.Empty;
    private int                   _editPathGroupId;
    private List<RuleEditModel>   _editRules = [];
    private HashSet<int>          _editOriginalRuleIds = [];
    private bool                  _saving;

    private sealed class RuleEditModel
    {
        public int                      Id;        public int                      Grade;
        public HashSet<int>             JobIds = [];
        public int?                     RetYears, RetMonths, ExpYears, ExpMonths;
        public bool                     Manager, Assess, Tech, Case, English, Committee;
        public int?                     ProjectTarget;
    }

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
            _loadError = Loc["gradeManagement.loadFailed"];
            Snackbar.Add(_loadError, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void TogglePath(int id) { if (!_expandedIds.Add(id)) _expandedIds.Remove(id); }

    private bool IsEditingPath(int pathId) => _editPathId == pathId;


    private async Task OpenAddCareerPathDialog()
    {
        if (_userGroups.Count == 0)
        {
            Snackbar.Add(Loc["gradeManagement.noUserGroups"], Severity.Warning);
            return;
        }

        var dialog = await DialogServiceRef.ShowAsync<CareerPathDialog>(Loc["careerPathDialog.addTitle"],
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
                Snackbar.Add(Loc["gradeManagement.careerPathCreated"], Severity.Success);
            }
            else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["gradeManagement.careerPathCreateFailed"], Severity.Error);
        }
    }

    private async Task DeleteCareerPath(CareerPathDto path)
    {
        if (await DialogServiceRef.ShowMessageBox(Loc["gradeManagement.deleteCareerPathTitle"],
            string.Format(Loc["gradeManagement.deleteCareerPathConfirm"], path.Name),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var resp = await OrgService.DeleteCareerPathAsync(path.Id);
        if (resp.IsSuccessStatusCode) { await LoadData(); Snackbar.Add(Loc["gradeManagement.careerPathDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["gradeManagement.careerPathDeleteFailed"], Severity.Error);
    }


    private void BeginEditPath(CareerPathDto path)
    {
        if (_editPathId is not null) return;

        _editPathId          = path.Id;
        _editPathName        = path.Name;
        _editPathGroupId     = path.UserGroupId;
        _editOriginalRuleIds = path.Rules.Select(r => r.Id).ToHashSet();
        _editRules = path.Rules
            .OrderBy(r => r.Grade)
            .Select(r => new RuleEditModel
            {
                Id            = r.Id,
                Grade         = r.Grade,
                JobIds        = r.PositionJobIds.ToHashSet(),
                RetYears      = r.MinRoleTime.Years,
                RetMonths     = r.MinRoleTime.Months,
                ExpYears      = r.MinExperience.Years,
                ExpMonths     = r.MinExperience.Months,
                Manager       = r.ManagerPerformanceEvaluation,
                Assess        = r.AssessmentCenterApplication,
                Tech          = r.TechnicalInterview,
                Case          = r.CaseStudy,
                English       = r.EnglishProficiency,
                Committee     = r.CommitteeApproval,
                ProjectTarget = r.ProjectObjective,
            })
            .ToList();

        _expandedIds.Add(path.Id);
    }

    private void CancelPathEdit()
    {
        _editPathId = null;
        _editRules  = [];
        _editOriginalRuleIds = [];
    }

    private void AddRuleRow()
    {
        if (_editPathId is null) return;
        var nextGrade = _editRules.Count > 0 ? _editRules.Max(r => r.Grade) + 1 : 1;
        _editRules.Add(new RuleEditModel { Id = 0, Grade = nextGrade });
    }

    private void RemoveLastRuleRow()
    {
        if (_editRules.Count == 0) return;
        var last = _editRules.OrderBy(r => r.Grade).Last();
        _editRules.Remove(last);
    }

    private bool IsLastRule(RuleEditModel rule) =>
        _editRules.Count > 0 && _editRules.OrderBy(r => r.Grade).Last() == rule;

    private bool CanSavePath =>
        !string.IsNullOrWhiteSpace(_editPathName)
        && _editPathGroupId != 0
        && _editRules.All(r => r.JobIds.Count > 0)
        && !_saving;

    private async Task SavePathEdit()
    {
        if (_editPathId is null || !CanSavePath) return;

        _saving = true;
        try
        {
            var pathResp = await OrgService.UpdateCareerPathAsync(new CareerPathDto
            {
                Id          = _editPathId.Value,
                Name        = _editPathName.Trim(),
                UserGroupId = _editPathGroupId,
            });
            if (!pathResp.IsSuccessStatusCode)
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(pathResp) ?? Loc["gradeManagement.careerPathUpdateFailed"], Severity.Error);
                return;
            }

            var keptIds = _editRules.Where(r => r.Id != 0).Select(r => r.Id).ToHashSet();
            foreach (var removedId in _editOriginalRuleIds.Where(id => !keptIds.Contains(id)))
            {
                var delResp = await OrgService.DeleteCareerMapRuleAsync(removedId);
                if (!delResp.IsSuccessStatusCode)
                {
                    Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(delResp) ?? Loc["gradeManagement.gradeDeleteFailed"], Severity.Error);
                    return;
                }
            }

            foreach (var r in _editRules)
            {
                var dto = ToRuleDto(r, _editPathId.Value);
                var resp = r.Id == 0
                    ? await OrgService.CreateCareerMapRuleAsync(dto)
                    : await OrgService.UpdateCareerMapRuleAsync(dto);
                if (!resp.IsSuccessStatusCode)
                {
                    Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["gradeManagement.gradeSaveFailed"], Severity.Error);
                    return;
                }
            }

            CancelPathEdit();
            await LoadData();
            Snackbar.Add(Loc["gradeManagement.careerPathSaved"], Severity.Success);
        }
        finally
        {
            _saving = false;
        }
    }

    private static CareerMapRuleDto ToRuleDto(RuleEditModel r, int pathId) => new()
    {
        Id                           = r.Id,
        CareerPathId                 = pathId,
        Grade                        = r.Grade,
        PositionJobIds               = [.. r.JobIds],
        MinRoleTime                  = new RoleDurationDto { Years = r.RetYears, Months = r.RetMonths },
        MinExperience                = new RoleDurationDto { Years = r.ExpYears, Months = r.ExpMonths },
        ManagerPerformanceEvaluation = r.Manager,
        AssessmentCenterApplication  = r.Assess,
        TechnicalInterview           = r.Tech,
        CaseStudy                    = r.Case,
        EnglishProficiency           = r.English,
        CommitteeApproval            = r.Committee,
        ProjectObjective             = r.ProjectTarget,
    };


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
