using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class Employee
{
    private bool _careerSaving;

    private async Task LoadCareerDataAsync()
    {
        try
        {
            var tCompanies  = OrgService.GetCompaniesAsync();
            var tDepts      = OrgService.GetDepartmentsAsync();
            var tTeams      = OrgService.GetTeamsAsync();
            var tJobs       = OrgService.GetJobsAsync();
            var tPaths      = OrgService.GetCareerPathsAsync();

            await Task.WhenAll(tCompanies, tDepts, tTeams, tJobs, tPaths);

            _career.Companies   = tCompanies.Result;
            _career.AllDepts    = tDepts.Result;
            _career.AllTeams    = tTeams.Result;
            _career.AllJobs     = tJobs.Result;
            _career.CareerPaths = tPaths.Result;
            _career.MarkLoaded();
            _ = LoadCvDocumentsAsync();

            _career.CompanyId   = _profile?.Company?.Id;
            _career.DeptId      = _profile?.Department?.Id;
            _career.TeamId      = _profile?.Team?.Id;
            _career.PathId      = _profile?.CareerPathId;
            _career.JobId       = _profile?.Job?.Id;
            _career.Grade       = _profile?.Grade;

            _career.FilteredDepts = FilteredDeptsForCompany(_career.CompanyId);
            _career.FilteredTeams = FilteredTeamsForDept(_career.DeptId);
            PrefillManagerFromProfile();

            if (_career.PathId.HasValue)
            {
                var path = _career.CareerPaths.FirstOrDefault(p => p.Id == _career.PathId);
                if (path != null)
                {
                    _career.PathJobs = path.Rules.SelectMany(r => r.PositionJobs).DistinctBy(j => j.Id).ToList();
                    if (_career.JobId.HasValue)
                        _career.JobGrades = path.Rules
                            .Where(r => r.PositionJobIds.Contains(_career.JobId.Value))
                            .Select(r => r.Grade).OrderBy(g => g).ToList();
                }
            }

            StateHasChanged();
        }
        catch (Exception)
        {
            Snackbar.Add(Loc["employeeProfile.careerLoadFailed"], Severity.Error);
        }
    }

    private void PrefillManagerFromProfile()
    {
        _career.SelectedManager = _profile?.ReportsTo is { } r
            ? new UserSearchResultDto
            {
                Id                     = r.Id,
                FirstName              = r.FirstName,
                LastName               = r.LastName,
                AvatarColorIndex       = r.AvatarColorIndex,
                HasAvatar              = r.HasAvatar,
                AvatarUpdatedAt        = r.AvatarUpdatedAt,
                OrganizationPositionId = r.OrganizationPositionId,
            }
            : null;
    }

    private async Task OpenCareerAssignmentDialogAsync(PositionHistoryDto? current)
    {
        if (_userId <= 0) return;

        var initial = new CareerAssignmentDialog.CareerAssignmentResult
        {
            ManagerId    = _career.SelectedManager?.Id,
            Manager      = _career.SelectedManager,
            CompanyId    = _profile?.Company?.Id,
            DepartmentId = _profile?.Department?.Id,
            TeamId       = _profile?.Team?.Id,
            PathId       = _profile?.CareerPathId,
            JobId        = _profile?.Job?.Id,
            Grade        = _profile?.Grade,
            StartDate    = current?.StartDate ?? _profile?.PositionStartDate ?? DateTime.Today,
            EndDate      = current?.EndDate
        };

        var parameters = new DialogParameters
        {
            [nameof(CareerAssignmentDialog.Companies)]     = _career.Companies,
            [nameof(CareerAssignmentDialog.AllDepts)]      = _career.AllDepts,
            [nameof(CareerAssignmentDialog.AllTeams)]      = _career.AllTeams,
            [nameof(CareerAssignmentDialog.CareerPaths)]   = _career.CareerPaths,
            [nameof(CareerAssignmentDialog.ExcludeUserId)] = _userId,
            [nameof(CareerAssignmentDialog.InitialModel)]  = initial,
            [nameof(CareerAssignmentDialog.IsFirstAssignment)] = current is null
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog  = await DialogService.ShowAsync<CareerAssignmentDialog>(
            current is null ? Loc["employeeProfile.assignPosition"] : Loc["employeeProfile.editCareerAssignment"], parameters, options);
        var result = await dialog.Result;
        if (result is null || result.Canceled || result.Data is not CareerAssignmentDialog.CareerAssignmentResult data) return;

        await SaveAssignmentAsync(data);
    }

    private async Task SaveAssignmentAsync(CareerAssignmentDialog.CareerAssignmentResult data)
    {
        if (_careerSaving) return;

        var manager     = data.Manager;
        var needNewNode = manager is { OrganizationPositionId: not null } && _profile?.OrganizationPositionId is null;
        string? newPositionName = null;
        if (needNewNode)
        {
            var p = new DialogParameters
            {
                [nameof(PositionNameDialog.ManagerName)] = AppFormatter.BuildFullName(manager!.FirstName, manager.LastName)
            };
            var o  = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseOnEscapeKey = true };
            var pd = await DialogService.ShowAsync<PositionNameDialog>(Loc["employeeProfile.newPosition"], p, o);
            var pr = await pd.Result;
            if (pr is not { Canceled: false, Data: string name } || string.IsNullOrWhiteSpace(name)) return;
            newPositionName = name;
        }

        _careerSaving = true;
        try
        {
            var dto = new UpdateCareerAssignmentDto
            {
                CompanyId       = data.CompanyId,
                DepartmentId    = data.DepartmentId,
                TeamId          = data.TeamId,
                CareerPathId    = data.PathId,
                JobId           = data.JobId,
                Grade           = data.Grade,
                ManagerId       = data.ManagerId,
                NewPositionName = newPositionName
            };
            var resp = await UserService.UpdateCareerAssignmentAsync(_userId, dto);
            if (!resp.IsSuccessStatusCode)
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["employeeProfile.assignmentSaveFailed"], Severity.Error);
                return;
            }

            var posDto = new UpdatePositionHistoryDto
            {
                CompanyId    = data.CompanyId,
                DepartmentId = data.DepartmentId,
                TeamId       = data.TeamId,
                StartDate    = data.StartDate ?? DateTime.Today,
                EndDate      = data.EndDate
            };
            await UserService.UpdateCurrentPositionAsync(_userId, posDto);

            await RefreshProfileAsync();
            ApplyProfileToCareerTab();
            Snackbar.Add(Loc["employeeProfile.assignmentSaved"], Severity.Success);
        }
        finally { _careerSaving = false; }
    }

    private async Task OpenPromotionDialogAsync()
    {
        if (_userId <= 0) return;

        var path = _profile?.CareerPathId is int id ? _career.CareerPaths.FirstOrDefault(p => p.Id == id) : null;
        if (path is null)
        {
            Snackbar.Add(Loc["employeeProfile.assignCareerPathFirst"], Severity.Warning);
            return;
        }

        var parameters = new DialogParameters
        {
            [nameof(PromotionDialog.Path)]  = path,
            [nameof(PromotionDialog.JobId)] = _profile?.Job?.Id,
            [nameof(PromotionDialog.Grade)] = _profile?.Grade
        };
        var dialog = await DialogService.ShowAsync<PromotionDialog>(Loc["employeeProfile.promotionDialogTitle"], parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is null || result.Canceled || result.Data is not PromotionDialog.PromotionResult data) return;

        if (_careerSaving) return;
        _careerSaving = true;
        try
        {
            var dto = new UpdateCareerAssignmentDto
            {
                CompanyId    = _profile?.Company?.Id,
                DepartmentId = _profile?.Department?.Id,
                TeamId       = _profile?.Team?.Id,
                CareerPathId = _profile?.CareerPathId,
                JobId        = data.JobId,
                Grade        = data.Grade
            };
            var resp = await UserService.UpdateCareerAssignmentAsync(_userId, dto);
            if (resp.IsSuccessStatusCode)
            {
                await RefreshProfileAsync();
                ApplyProfileToCareerTab();
                Snackbar.Add(Loc["employeeProfile.employeePromoted"], Severity.Success);
            }
            else
            {
                Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["employeeProfile.promotionFailed"], Severity.Error);
            }
        }
        finally { _careerSaving = false; }
    }

    private CareerPathDto? AssignedPath()
        => _profile?.CareerPathId is int id ? _career.CareerPaths.FirstOrDefault(p => p.Id == id) : null;

    private List<CareerPathRuleDto> PathRulesOrdered()
        => GradeProgressCalculator.PathRulesOrdered(AssignedPath());

    private CareerPathRuleDto? NextGradeRule()
        => GradeProgressCalculator.NextGradeRule(AssignedPath(), _profile?.Grade);

    private string FormatPositionRange(PositionHistoryDto p)
        => $"{p.StartDate:MMM d, yyyy} - {(p.EndDate is { } e ? e.ToString("MMM d, yyyy") : Loc["employeeProfile.present"])}";

    private List<GradeRequirement> NextGradeRequirements()
        => GradeProgressCalculator.NextGradeRequirements(
            Loc, AssignedPath(), _profile?.Grade, _profile?.UserStartDate, _profile?.PositionStartDate);

    private List<GradeBenefit> CurrentGradeBenefits()
        => GradeProgressCalculator.CurrentGradeBenefits(Loc, AssignedPath(), _profile?.Grade);

    private void ApplyProfileToCareerTab()
    {
        _career.CompanyId     = _profile?.Company?.Id;
        _career.DeptId        = _profile?.Department?.Id;
        _career.TeamId        = _profile?.Team?.Id;
        _career.PathId        = _profile?.CareerPathId;
        _career.JobId         = _profile?.Job?.Id;
        _career.Grade         = _profile?.Grade;
        PrefillManagerFromProfile();
        _career.FilteredDepts = FilteredDeptsForCompany(_career.CompanyId);
        _career.FilteredTeams = FilteredTeamsForDept(_career.DeptId);

        var path = _career.PathId.HasValue
            ? _career.CareerPaths.FirstOrDefault(p => p.Id == _career.PathId)
            : null;

        _career.PathJobs  = path is null ? [] : path.Rules.SelectMany(r => r.PositionJobs).DistinctBy(j => j.Id).ToList();
        _career.JobGrades = (path is null || !_career.JobId.HasValue)
            ? []
            : path.Rules.Where(r => r.PositionJobIds.Contains(_career.JobId.Value))
                .Select(r => r.Grade).OrderBy(g => g).ToList();
    }

    private List<DepartmentDto> FilteredDeptsForCompany(int? companyId)
        => companyId is null
            ? []
            : _career.AllDepts.Where(d => d.CompanyId == companyId).ToList();

    private List<TeamDto> FilteredTeamsForDept(int? deptId)
        => deptId.HasValue
            ? _career.AllTeams.Where(t => t.DepartmentId == deptId).ToList()
            : [];
}
