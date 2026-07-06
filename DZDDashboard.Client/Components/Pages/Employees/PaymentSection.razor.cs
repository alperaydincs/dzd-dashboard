using DZDDashboard.Client.Components.Pages.Employees.Dialogs;
using DZDDashboard.Client.Services;
using DZDDashboard.Client.Theme;
using DZDDashboard.Common.Constants;
using DZDDashboard.Common.DTOs;
using DZDDashboard.Common.Utils;
using DZDDashboard.Common.Validation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DZDDashboard.Client.Components.Pages.Employees;

public partial class PaymentSection
{
    [Parameter, EditorRequired] public int UserId { get; set; }
    [Parameter] public bool SelfService { get; set; }

    [Inject] private IPaymentClientService                       PaymentService           { get; set; } = default!;
    [Inject] private IDialogService                             DialogService            { get; set; } = default!;
    [Inject] private ISnackbar                                  Snackbar                 { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.AppLocalizer Loc                    { get; set; } = default!;
    [Inject] private DZDDashboard.Client.Localization.DomainLocalizer Domain              { get; set; } = default!;

    private static readonly DialogOptions SmallEscDialog = new() { MaxWidth = MaxWidth.Small, FullWidth = true, CloseOnEscapeKey = true };

    private bool _loading = true;
    private string? _loadError;
    private EmployeePaymentDto? _payment;

    private int _activeTab;

    private string GetRelationTypeName(string? code)          => Domain.Label(DomainCategories.RelationType, code);
    private string GetAdditionalPaymentTypeName(string? code) => Domain.Label(DomainCategories.AdditionalPaymentType, code);
    private string GetDeductionTypeName(string? code)         => Domain.Label(DomainCategories.DeductionType, code);
    private string GetBenefitTypeName(string? code)           => Domain.Label(DomainCategories.BenefitType, code);
    private string GetPayTypeName(string? code)                => Domain.Label(DomainCategories.PayType, code);
    private string GetPaymentPeriodName(string? code)           => Domain.Label(DomainCategories.PaymentPeriod, code);
    private string GetAdditionalPaymentPeriodName(string? code) => Domain.Label(DomainCategories.AdditionalPaymentPeriod, code);
    private string GetCurrencyName(string? code)                 => Domain.Label(DomainCategories.Currency, code);

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (UserId > 0 && _payment is not null && _payment.EmployeeId != UserId)
            await LoadAsync();
    }

    private async Task LoadAsync()
    {
        if (UserId <= 0) return;

        _loading   = true;
        _loadError = null;
        try
        {
            _payment = SelfService
                ? await PaymentService.GetMyPaymentAsync()
                : await PaymentService.GetEmployeePaymentAsync(UserId);
            if (_payment is null) _loadError = Loc["payment.loadFailed"];
        }
        catch (Exception)
        {
            _loadError = Loc["payment.loadFailedRetry"];
        }
        finally
        {
            _loading = false;
        }
    }

    private static string FormatMoney(decimal amount, string currency) => $"{amount:N2}{Currencies.Symbol(currency)}";

    private static string FormatMoneyTotals(List<CurrencyAmountDto> totals)
        => totals.Count == 0 ? "0,00" : string.Join(" + ", totals.Select(t => FormatMoney(t.Amount, t.Currency)));

    private static string BenefitBadgeClass(string benefitType) => "amber";

    private static string BenefitIcon(string benefitType) => benefitType switch
    {
        BenefitTypes.PrivateHealthInsurance => DzdIcons.Heart,
        BenefitTypes.PrivatePension         => DzdIcons.FileText,
        _                                   => DzdIcons.Wallet
    };

    private static bool HasBesDetails(BenefitRecordDto record) =>
        record.EmployeeContributionAmount.HasValue
        || record.EmployerContributionAmount.HasValue
        || !string.IsNullOrWhiteSpace(record.ProviderName)
        || !string.IsNullOrWhiteSpace(record.PolicyNumber);

    private SalaryRecordDto? CurrentSalary =>
        _payment?.SalaryHistory.FirstOrDefault(s => s.EndDate is null) ?? _payment?.SalaryHistory.FirstOrDefault();


    private async Task OpenAddSalaryDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<SalaryRecordDialog>(Loc["payment.addSalaryDialogTitle"],
            new DialogParameters { [nameof(SalaryRecordDialog.ConfirmButtonText)] = Loc["common.add"] }, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: SalaryRecordDto dto }) return;

        var created = await PaymentService.CreateSalaryRecordAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add(Loc["payment.salaryAdded"], Severity.Success); }
        else Snackbar.Add(Loc["payment.salaryAddFailed"], Severity.Error);
    }

    private async Task OpenEditSalaryDialogAsync(SalaryRecordDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(SalaryRecordDialog.InitialModel)]     = record,
            [nameof(SalaryRecordDialog.ConfirmButtonText)] = Loc["payment.saveChanges"]
        };
        var dialog = await DialogService.ShowAsync<SalaryRecordDialog>(Loc["payment.editSalaryDialogTitle"], parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: SalaryRecordDto dto }) return;

        var resp = await PaymentService.UpdateSalaryRecordAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.salaryUpdated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.salaryUpdateFailed"], Severity.Error);
    }

    private async Task DeleteSalaryRecordAsync(SalaryRecordDto record)
    {
        if (await DialogService.ShowMessageBoxAsync(Loc["payment.deleteSalaryDialogTitle"],
            string.Format(Loc["payment.deleteSalaryConfirm"], FormatMoney(record.Amount, record.Currency), GetPaymentPeriodName(record.Period), AppFormatter.FormatDate(record.StartDate)),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var resp = await PaymentService.DeleteSalaryRecordAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.salaryDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.salaryDeleteFailed"], Severity.Error);
    }


    private async Task OpenAddBenefitDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<BenefitRecordDialog>(Loc["payment.addBenefitDialogTitle"],
            new DialogParameters { [nameof(BenefitRecordDialog.ConfirmButtonText)] = Loc["common.add"] }, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitRecordDto dto }) return;

        var created = await PaymentService.CreateBenefitRecordAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add(Loc["payment.benefitAdded"], Severity.Success); }
        else Snackbar.Add(Loc["payment.benefitAddFailed"], Severity.Error);
    }

    private async Task OpenEditBenefitDialogAsync(BenefitRecordDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(BenefitRecordDialog.InitialModel)]     = record,
            [nameof(BenefitRecordDialog.ConfirmButtonText)] = Loc["payment.saveChanges"]
        };
        var dialog = await DialogService.ShowAsync<BenefitRecordDialog>(Loc["payment.editBenefitDialogTitle"], parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitRecordDto dto }) return;

        var resp = await PaymentService.UpdateBenefitRecordAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.benefitUpdated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.benefitUpdateFailed"], Severity.Error);
    }

    private async Task OpenAddDependentDialogAsync(BenefitRecordDto record)
    {
        if (record.Dependents.Count >= ValidationConstants.MaxBenefitDependents)
        {
            Snackbar.Add(string.Format(Loc["payment.maxDependentsWarning"], ValidationConstants.MaxBenefitDependents), Severity.Warning);
            return;
        }

        var dialog = await DialogService.ShowAsync<DependentDialog>(Loc["payment.addDependentDialogTitle"],
            new DialogParameters { [nameof(DependentDialog.ConfirmButtonText)] = Loc["common.add"] }, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitDependentDto dependent }) return;

        var updated = record with { Dependents = [.. record.Dependents, dependent] };

        var resp = await PaymentService.UpdateBenefitRecordAsync(UserId, record.Id, updated);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.dependentAdded"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.dependentAddFailed"], Severity.Error);
    }

    private async Task DeleteBenefitRecordAsync(BenefitRecordDto record)
    {
        if (await DialogService.ShowMessageBoxAsync(Loc["payment.deleteBenefitDialogTitle"],
            string.Format(Loc["payment.deleteBenefitConfirm"], GetBenefitTypeName(record.BenefitType), FormatMoney(record.Amount, record.Currency), GetPaymentPeriodName(record.Period)),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var resp = await PaymentService.DeleteBenefitRecordAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.benefitDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.benefitDeleteFailed"], Severity.Error);
    }


    private async Task OpenAddAdditionalPaymentDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<AdditionalPaymentDialog>(Loc["payment.addAdditionalPaymentDialogTitle"],
            new DialogParameters { [nameof(AdditionalPaymentDialog.ConfirmButtonText)] = Loc["common.add"] }, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: AdditionalPaymentDto dto }) return;

        var created = await PaymentService.CreateAdditionalPaymentAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add(Loc["payment.additionalPaymentAdded"], Severity.Success); }
        else Snackbar.Add(Loc["payment.additionalPaymentAddFailed"], Severity.Error);
    }

    private async Task OpenEditAdditionalPaymentDialogAsync(AdditionalPaymentDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(AdditionalPaymentDialog.InitialModel)]     = record,
            [nameof(AdditionalPaymentDialog.ConfirmButtonText)] = Loc["employeeProfile.update"]
        };
        var dialog = await DialogService.ShowAsync<AdditionalPaymentDialog>(Loc["payment.editAdditionalPaymentDialogTitle"], parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: AdditionalPaymentDto dto }) return;

        var resp = await PaymentService.UpdateAdditionalPaymentAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.additionalPaymentUpdated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.additionalPaymentUpdateFailed"], Severity.Error);
    }

    private async Task DeleteAdditionalPaymentAsync(AdditionalPaymentDto record)
    {
        if (await DialogService.ShowMessageBoxAsync(Loc["payment.deleteAdditionalPaymentDialogTitle"],
            string.Format(Loc["payment.deleteAdditionalPaymentConfirm"], GetAdditionalPaymentTypeName(record.PaymentType), FormatMoney(record.Amount, record.Currency)),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var resp = await PaymentService.DeleteAdditionalPaymentAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.additionalPaymentDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.additionalPaymentDeleteFailed"], Severity.Error);
    }


    private async Task OpenAddDeductionDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<DeductionDialog>(Loc["payment.addDeductionDialogTitle"],
            new DialogParameters { [nameof(DeductionDialog.ConfirmButtonText)] = Loc["common.add"] }, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: DeductionDto dto }) return;

        var created = await PaymentService.CreateDeductionAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add(Loc["payment.deductionAdded"], Severity.Success); }
        else Snackbar.Add(Loc["payment.deductionAddFailed"], Severity.Error);
    }

    private async Task OpenEditDeductionDialogAsync(DeductionDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(DeductionDialog.InitialModel)]      = record,
            [nameof(DeductionDialog.ConfirmButtonText)] = Loc["employeeProfile.update"]
        };
        var dialog = await DialogService.ShowAsync<DeductionDialog>(Loc["payment.editDeductionDialogTitle"], parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: DeductionDto dto }) return;

        var resp = await PaymentService.UpdateDeductionAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.deductionUpdated"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.deductionUpdateFailed"], Severity.Error);
    }

    private async Task DeleteDeductionAsync(DeductionDto record)
    {
        if (await DialogService.ShowMessageBoxAsync(Loc["payment.deleteDeductionDialogTitle"],
            string.Format(Loc["payment.deleteDeductionConfirm"], GetDeductionTypeName(record.DeductionType), FormatMoney(record.Amount, record.Currency), GetAdditionalPaymentPeriodName(record.Period)),
            yesText: Loc["payment.delete"], cancelText: Loc["common.cancel"]) != true) return;

        var resp = await PaymentService.DeleteDeductionAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add(Loc["payment.deductionDeleted"], Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? Loc["payment.deductionDeleteFailed"], Severity.Error);
    }
}
