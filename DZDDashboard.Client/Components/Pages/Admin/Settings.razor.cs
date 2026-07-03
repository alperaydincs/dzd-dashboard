using DZDDashboard.Client.Components.OrgChart;
using DZDDashboard.Client.Components.Pages.Admin.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Admin;

public partial class Settings
{
    [Inject] private IDialogService DialogServiceRef { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IOrganizationClientService OrgService { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.AppLocalizer Loc                  { get; set; } = default!;

    private bool _loading = true;
    private int  _activeSettingsTabIndex;

    private List<OrgChartNodeItem>       _orgNodes              = [];
    private List<OrganizationPositionDto> _organizationPositions = [];

    private List<CompanyDto>         _companies        = [];
    private List<DepartmentDto>      _departments      = [];
    private List<TeamDto>            _teams            = [];
    private List<PayrollLocationDto> _payrollLocations = [];
    private List<UserGroupDto>       _userGroups       = [];
    private List<JobDto>             _jobs             = [];


    private bool _orgStructureExpanded;
    private bool _jobTitlesExpanded;
    private bool _employeeGroupsExpanded;
    private bool _payrollLocationsExpanded;

    private HashSet<int> _expandedCompanyIds = [];
    private HashSet<int> _expandedDeptIds    = [];

    private bool   _addingCompany;
    private string _newCompanyName         = string.Empty;
    private int?   _addingDeptForCompanyId;
    private string _newDeptName            = string.Empty;
    private int?   _addingTeamForDeptId;
    private string _newTeamName            = string.Empty;

    protected override async Task OnInitializedAsync() => await LoadData();

    private async Task LoadData()
    {
        _loading = true;
        try
        {
            var t1 = OrgService.GetOrganizationPositionsAsync();
            var t2 = OrgService.GetCompaniesAsync();
            var t3 = OrgService.GetDepartmentsAsync();
            var t4 = OrgService.GetTeamsAsync();
            var t5 = OrgService.GetPayrollLocationsAsync();
            var t6 = OrgService.GetUserGroupsAsync();
            var t7 = OrgService.GetJobsAsync();

            await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7);

            _organizationPositions = t1.Result;
            _companies             = t2.Result;
            _departments           = t3.Result;
            _teams                 = t4.Result;
            _payrollLocations      = t5.Result;
            _userGroups            = t6.Result;
            _jobs                  = t7.Result;

            BuildOrgChart(_organizationPositions);
        }
        catch (Exception)
        {
            Snackbar.Add(Loc["settings.dataLoadFailed"], Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void SetSettingsTab(int index) => _activeSettingsTabIndex = index;

    private void ToggleCompany(int id) { if (!_expandedCompanyIds.Add(id)) _expandedCompanyIds.Remove(id); }
    private void ToggleDept(int id)    { if (!_expandedDeptIds.Add(id))    _expandedDeptIds.Remove(id); }

    private void CancelAddCompany() { _addingCompany = false; _newCompanyName = string.Empty; }

    private async Task OnCompanyKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")  await SaveCompany();
        else if (e.Key == "Escape") CancelAddCompany();
    }

    private async Task SaveCompany()
    {
        if (string.IsNullOrWhiteSpace(_newCompanyName)) return;
        var response = await OrgService.CreateCompanyAsync(new CompanyDto { Name = _newCompanyName.Trim() });
        if (response.IsSuccessStatusCode) { _addingCompany = false; _newCompanyName = string.Empty; await RefreshCompaniesAsync(); Snackbar.Add(Loc["settings.companyCreated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.companyCreateFailed"], Severity.Error);
    }

    private void StartAddDept(int companyId)
    {
        _addingDeptForCompanyId = companyId;
        _newDeptName = string.Empty;
        _expandedCompanyIds.Add(companyId);
    }

    private void CancelAddDept() { _addingDeptForCompanyId = null; _newDeptName = string.Empty; }

    private async Task OnDeptKeyDown(KeyboardEventArgs e, int companyId)
    {
        if (e.Key == "Enter") await SaveDept(companyId);
        else if (e.Key == "Escape") CancelAddDept();
    }

    private async Task SaveDept(int companyId)
    {
        if (string.IsNullOrWhiteSpace(_newDeptName)) return;
        var response = await OrgService.CreateDepartmentAsync(new DepartmentDto { Name = _newDeptName.Trim(), CompanyId = companyId });
        if (response.IsSuccessStatusCode) { _addingDeptForCompanyId = null; _newDeptName = string.Empty; await RefreshDepartmentsAsync(); Snackbar.Add(Loc["settings.departmentCreated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.departmentCreateFailed"], Severity.Error);
    }

    private void StartAddTeam(int deptId)
    {
        _addingTeamForDeptId = deptId;
        _newTeamName = string.Empty;
        _expandedDeptIds.Add(deptId);
    }

    private void CancelAddTeam() { _addingTeamForDeptId = null; _newTeamName = string.Empty; }

    private async Task OnTeamKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && _addingTeamForDeptId.HasValue) await SaveTeam(_addingTeamForDeptId.Value);
        else if (e.Key == "Escape") CancelAddTeam();
    }

    private async Task SaveTeam(int deptId)
    {
        if (string.IsNullOrWhiteSpace(_newTeamName)) return;
        var response = await OrgService.CreateTeamAsync(new TeamDto { Name = _newTeamName.Trim(), DepartmentId = deptId });
        if (response.IsSuccessStatusCode) { _addingTeamForDeptId = null; _newTeamName = string.Empty; await RefreshTeamsAsync(); Snackbar.Add(Loc["settings.teamCreated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.teamCreateFailed"], Severity.Error);
    }

    private static readonly DialogOptions DefaultDialogOptions = new() { MaxWidth = MaxWidth.Small, FullWidth = true };

    private async Task RefreshCompaniesAsync()        { _companies        = await OrgService.GetCompaniesAsync(); }
    private async Task RefreshDepartmentsAsync()      { _departments      = await OrgService.GetDepartmentsAsync(); }
    private async Task RefreshTeamsAsync()            { _teams            = await OrgService.GetTeamsAsync(); }
    private async Task RefreshJobsAsync()             { _jobs             = await OrgService.GetJobsAsync(); }
    private async Task RefreshUserGroupsAsync()       { _userGroups       = await OrgService.GetUserGroupsAsync(); }
    private async Task RefreshPayrollLocationsAsync() { _payrollLocations = await OrgService.GetPayrollLocationsAsync(); }
    private async Task RefreshPositionsAsync()
    {
        _organizationPositions = await OrgService.GetOrganizationPositionsAsync();
        BuildOrgChart(_organizationPositions);
    }
    private async Task RefreshCompanyHierarchyAsync()
    {
        var t1 = OrgService.GetCompaniesAsync();
        var t2 = OrgService.GetDepartmentsAsync();
        var t3 = OrgService.GetTeamsAsync();
        await Task.WhenAll(t1, t2, t3);
        _companies    = t1.Result;
        _departments  = t2.Result;
        _teams        = t3.Result;
    }
    private async Task RefreshDeptHierarchyAsync()
    {
        var t1 = OrgService.GetDepartmentsAsync();
        var t2 = OrgService.GetTeamsAsync();
        await Task.WhenAll(t1, t2);
        _departments = t1.Result;
        _teams       = t2.Result;
    }

    private async Task ShowCrudDialogAsync<TDialog>(string title, DialogParameters parameters,
        Func<Task>? refresh = null)
        where TDialog : Microsoft.AspNetCore.Components.ComponentBase
    {
        var dialog = await DialogServiceRef.ShowAsync<TDialog>(title, parameters, DefaultDialogOptions);
        if (await dialog.Result is { Canceled: false }) await (refresh ?? LoadData)();
    }

    private async Task ConfirmDeleteAsync(string entityType, string entityName,
        Func<Task<HttpResponseMessage>> deleteFunc, Func<Task>? refresh = null)
    {
        if (await DialogServiceRef.ShowMessageBox(string.Format(Loc["settings.deleteEntityTitle"], entityType), string.Format(Loc["settings.deleteEntityConfirm"], entityName),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var response = await deleteFunc();
        if (response.IsSuccessStatusCode) { await (refresh ?? LoadData)(); Snackbar.Add(string.Format(Loc["settings.entityDeleted"], entityType), Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? string.Format(Loc["settings.entityDeleteFailed"], entityType.ToLowerInvariant()), Severity.Error);
    }

    private Task OpenEditCompanyDialog(CompanyDto c)
        => ShowCrudDialogAsync<CompanyDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["employeeProfile.company"]),
            new() { ["Company"] = c with { } },
            RefreshCompaniesAsync);

    private Task OpenEditDeptDialog(DepartmentDto d)
        => ShowCrudDialogAsync<DepartmentDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["employeeProfile.department"]),
            new() { ["Department"] = d with { }, ["Companies"] = _companies },
            RefreshDepartmentsAsync);

    private Task OpenEditTeamDialog(TeamDto t)
        => ShowCrudDialogAsync<TeamDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["settings.team"]),
            new() { ["Team"] = t with { }, ["Companies"] = _companies, ["Departments"] = _departments },
            RefreshTeamsAsync);

    private Task DeleteCompany(CompanyDto c)       => ConfirmDeleteAsync(Loc["employeeProfile.company"],    c.Name ?? "", () => OrgService.DeleteCompanyAsync(c.Id),    RefreshCompanyHierarchyAsync);
    private Task DeleteDepartment(DepartmentDto d)  => ConfirmDeleteAsync(Loc["employeeProfile.department"], d.Name ?? "", () => OrgService.DeleteDepartmentAsync(d.Id), RefreshDeptHierarchyAsync);
    private Task DeleteTeam(TeamDto t)              => ConfirmDeleteAsync(Loc["settings.team"],       t.Name ?? "", () => OrgService.DeleteTeamAsync(t.Id),       RefreshTeamsAsync);

    private Task OpenAddJobDialog()             => ShowCrudDialogAsync<JobDialog>(string.Format(Loc["settings.addEntityTitle"], Loc["settings.jobTitle"]),  new() { ["Job"] = new JobDto() },  RefreshJobsAsync);
    private Task OpenEditJobDialog(JobDto j)    => ShowCrudDialogAsync<JobDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["settings.jobTitle"]), new() { ["Job"] = j with { } },   RefreshJobsAsync);
    private Task DeleteJob(JobDto j)            => ConfirmDeleteAsync(Loc["settings.jobTitle"], j.Name ?? "", () => OrgService.DeleteJobAsync(j.Id), RefreshJobsAsync);

    private Task OpenAddUserGroupDialog()               => ShowCrudDialogAsync<UserGroupDialog>(string.Format(Loc["settings.addEntityTitle"], Loc["settings.employeeGroupSingular"]),  new() { ["UserGroup"] = new UserGroupDto() }, RefreshUserGroupsAsync);
    private Task OpenEditUserGroupDialog(UserGroupDto g)  => ShowCrudDialogAsync<UserGroupDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["settings.employeeGroupSingular"]), new() { ["UserGroup"] = g with { } },        RefreshUserGroupsAsync);
    private Task DeleteUserGroup(UserGroupDto g)          => ConfirmDeleteAsync(Loc["settings.employeeGroupSingular"], g.GroupName ?? "", () => OrgService.DeleteUserGroupAsync(g.Id),     RefreshUserGroupsAsync);

    private Task OpenAddPayrollLocationDialog()                   => ShowCrudDialogAsync<PayrollLocationDialog>(string.Format(Loc["settings.addEntityTitle"], Loc["settings.payrollLocationSingular"]),  new() { ["PayrollLocation"] = new PayrollLocationDto() }, RefreshPayrollLocationsAsync);
    private Task OpenEditPayrollLocationDialog(PayrollLocationDto l) => ShowCrudDialogAsync<PayrollLocationDialog>(string.Format(Loc["settings.editEntityTitle"], Loc["settings.payrollLocationSingular"]), new() { ["PayrollLocation"] = l with { } },            RefreshPayrollLocationsAsync);
    private Task DeletePayrollLocation(PayrollLocationDto l)         => ConfirmDeleteAsync(Loc["settings.payrollLocationSingular"], l.Name ?? "", () => OrgService.DeletePayrollLocationAsync(l.Id),                  RefreshPayrollLocationsAsync);

    private void BuildOrgChart(List<OrganizationPositionDto> positions)
        => _orgNodes = OrgChartBuilder.Build(positions);

    private async Task AddChildNode(OrgChartNodeItem node)
        => await OpenPositionAddDialog(parentId: node.Position.Id);

    private async Task OpenPositionAddDialog(int? parentId = null)
    {
        var parameters = new DialogParameters
        {
            ["Name"]     = string.Empty,
            ["ParentId"] = parentId
        };
        var dialog = await DialogServiceRef.ShowAsync<OrganizationPositionDialog>(string.Format(Loc["settings.addEntityTitle"], Loc["settings.position"]), parameters, DefaultDialogOptions);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        if (result.Data is not OrganizationPositionDialog.Result data)
        {
            Snackbar.Add(Loc["settings.invalidDialogResult"], Severity.Error);
            return;
        }

        var createDto = new CreateOrganizationPositionDto { Name = data.Name, ParentId = data.ParentId };
        var response  = await OrgService.CreateOrganizationPositionAsync(createDto);

        if (response.IsSuccessStatusCode) { await RefreshPositionsAsync(); StateHasChanged(); Snackbar.Add(Loc["settings.positionCreated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.positionCreateFailed"], Severity.Error);
    }

    private async Task EditNode(OrgChartNodeItem node)
    {
        var parameters = new DialogParameters
        {
            ["PositionName"]  = node.Position.Name,
            ["CurrentUserId"] = node.User?.Id
        };

        var dialog = await DialogServiceRef.ShowAsync<PositionUserDialog>(Loc["settings.editPositionAndUser"], parameters, DefaultDialogOptions);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;
        if (result.Data is not PositionUserDialog.PositionUserDialogResult cast) return;

        var position = _organizationPositions.FirstOrDefault(p => p.Id == node.Position.Id);
        if (position == null) { Snackbar.Add(Loc["settings.positionNotFound"], Severity.Error); return; }

        if (position.Name == (cast.PositionName ?? position.Name) && (node.User?.Id ?? 0) == (cast.User?.Id ?? 0))
        {
            Snackbar.Add(Loc["settings.noChangesDetected"], Severity.Info);
            return;
        }

        var updateDto = new UpdateOrganizationPositionDto
        {
            Id       = position.Id,
            Name     = cast.PositionName ?? position.Name,
            ParentId = position.ParentId,
            UserId   = cast.User?.Id
        };

        var response = await OrgService.UpdateOrganizationPositionAsync(node.Position.Id, updateDto);
        if (response.IsSuccessStatusCode) { await RefreshPositionsAsync(); Snackbar.Add(Loc["settings.positionUpdated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.positionUpdateFailed"], Severity.Error);
    }

    private async Task DeleteNode(OrgChartNodeItem node)
    {
        if (node.User != null)
        {
            Snackbar.Add(string.Format(Loc["settings.cannotDeleteAssignedUser"], AppFormatter.BuildFullName(node.User.FirstName, node.User.LastName)), Severity.Warning);
            return;
        }

        if (node.Children is { Count: > 0 })
        {
            Snackbar.Add(Loc["settings.cannotDeleteChildPositions"], Severity.Warning);
            return;
        }

        if (await DialogServiceRef.ShowMessageBox(string.Format(Loc["settings.deleteEntityTitle"], Loc["settings.position"]), Loc["settings.areYouSure"], yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var response = await OrgService.DeleteOrganizationPositionAsync(node.Position.Id);
        if (response.IsSuccessStatusCode) { await RefreshPositionsAsync(); Snackbar.Add(Loc["settings.positionDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(response) ?? Loc["settings.positionDeleteFailed"], Severity.Error);
    }

}
