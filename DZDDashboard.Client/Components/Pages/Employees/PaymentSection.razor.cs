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

    [Inject] private IPaymentClientService PaymentService { get; set; } = default!;
    [Inject] private IDialogService        DialogService  { get; set; } = default!;
    [Inject] private ISnackbar             Snackbar       { get; set; } = default!;

    private static readonly DialogOptions SmallEscDialog = new() { MaxWidth = MaxWidth.Small, FullWidth = true, CloseOnEscapeKey = true };

    private bool _loading = true;
    private string? _loadError;
    private EmployeePaymentDto? _payment;

    private int _activeTab;

    protected override async Task OnInitializedAsync() => await LoadAsync();

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
            _payment = await PaymentService.GetEmployeePaymentAsync(UserId);
            if (_payment is null) _loadError = "Failed to load payment information.";
        }
        catch (Exception)
        {
            _loadError = "Failed to load payment information. Please refresh the page.";
        }
        finally
        {
            _loading = false;
        }
    }

    private static string FormatMoney(decimal amount, string currency) => $"{amount:N2} {currency}";

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

    private List<CurrencyAmountDto> AdditionalPaymentsTotal =>
        _payment?.AdditionalPayments
            .GroupBy(p => p.Currency)
            .Select(g => new CurrencyAmountDto(g.Key, g.Sum(p => p.Amount)))
            .ToList() ?? [];


    private async Task OpenAddSalaryDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<SalaryRecordDialog>("Add Salary Record", new DialogParameters(), SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: SalaryRecordDto dto }) return;

        var created = await PaymentService.CreateSalaryRecordAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add("Salary record added.", Severity.Success); }
        else Snackbar.Add("Failed to add salary record.", Severity.Error);
    }

    private async Task OpenEditSalaryDialogAsync(SalaryRecordDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(SalaryRecordDialog.InitialModel)]     = record,
            [nameof(SalaryRecordDialog.ConfirmButtonText)] = "Save Changes"
        };
        var dialog = await DialogService.ShowAsync<SalaryRecordDialog>("Edit Salary Record", parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: SalaryRecordDto dto }) return;

        var resp = await PaymentService.UpdateSalaryRecordAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Salary record updated.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update salary record.", Severity.Error);
    }

    private async Task DeleteSalaryRecordAsync(SalaryRecordDto record)
    {
        if (await DialogService.ShowMessageBox("Delete Salary Record",
            $"Delete the {FormatMoney(record.NetAmount, record.Currency)} / {record.Period} record starting {AppFormatter.FormatDate(record.StartDate)}?",
            yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await PaymentService.DeleteSalaryRecordAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Salary record deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete salary record.", Severity.Error);
    }


    private async Task OpenAddBenefitDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<BenefitRecordDialog>("Add New Benefit", new DialogParameters(), SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitRecordDto dto }) return;

        var created = await PaymentService.CreateBenefitRecordAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add("Benefit added.", Severity.Success); }
        else Snackbar.Add("Failed to add benefit.", Severity.Error);
    }

    private async Task OpenEditBenefitDialogAsync(BenefitRecordDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(BenefitRecordDialog.InitialModel)]     = record,
            [nameof(BenefitRecordDialog.ConfirmButtonText)] = "Save Changes"
        };
        var dialog = await DialogService.ShowAsync<BenefitRecordDialog>("Edit Benefit", parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitRecordDto dto }) return;

        var resp = await PaymentService.UpdateBenefitRecordAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Benefit updated.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update benefit.", Severity.Error);
    }

    private async Task OpenAddDependentDialogAsync(BenefitRecordDto record)
    {
        if (record.Dependents.Count >= ValidationConstants.MaxBenefitDependents)
        {
            Snackbar.Add($"A benefit may have at most {ValidationConstants.MaxBenefitDependents} dependents.", Severity.Warning);
            return;
        }

        var dialog = await DialogService.ShowAsync<DependentDialog>("Add Dependent", new DialogParameters(), SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: BenefitDependentDto dependent }) return;

        var updated = record with { Dependents = [.. record.Dependents, dependent] };

        var resp = await PaymentService.UpdateBenefitRecordAsync(UserId, record.Id, updated);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Dependent added.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to add dependent.", Severity.Error);
    }

    private async Task DeleteBenefitRecordAsync(BenefitRecordDto record)
    {
        if (await DialogService.ShowMessageBox("Delete Benefit",
            $"Delete the {record.BenefitType} benefit ({FormatMoney(record.Amount, record.Currency)} / {record.Period})?",
            yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await PaymentService.DeleteBenefitRecordAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Benefit deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete benefit.", Severity.Error);
    }


    private async Task OpenAddAdditionalPaymentDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<AdditionalPaymentDialog>("Add Additional Payment", new DialogParameters(), SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: AdditionalPaymentDto dto }) return;

        var created = await PaymentService.CreateAdditionalPaymentAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add("Additional payment added.", Severity.Success); }
        else Snackbar.Add("Failed to add additional payment.", Severity.Error);
    }

    private async Task OpenEditAdditionalPaymentDialogAsync(AdditionalPaymentDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(AdditionalPaymentDialog.InitialModel)]     = record,
            [nameof(AdditionalPaymentDialog.ConfirmButtonText)] = "Update"
        };
        var dialog = await DialogService.ShowAsync<AdditionalPaymentDialog>("Edit Additional Payment", parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: AdditionalPaymentDto dto }) return;

        var resp = await PaymentService.UpdateAdditionalPaymentAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Additional payment updated.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update additional payment.", Severity.Error);
    }

    private async Task DeleteAdditionalPaymentAsync(AdditionalPaymentDto record)
    {
        if (await DialogService.ShowMessageBox("Delete Additional Payment",
            $"Delete the {record.PaymentType} payment ({FormatMoney(record.Amount, record.Currency)})?",
            yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await PaymentService.DeleteAdditionalPaymentAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Additional payment deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete additional payment.", Severity.Error);
    }


    private async Task OpenAddDeductionDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<DeductionDialog>("Add Deduction", new DialogParameters(), SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: DeductionDto dto }) return;

        var created = await PaymentService.CreateDeductionAsync(UserId, dto);
        if (created is not null) { await LoadAsync(); Snackbar.Add("Deduction added.", Severity.Success); }
        else Snackbar.Add("Failed to add deduction.", Severity.Error);
    }

    private async Task OpenEditDeductionDialogAsync(DeductionDto record)
    {
        var parameters = new DialogParameters
        {
            [nameof(DeductionDialog.InitialModel)]      = record,
            [nameof(DeductionDialog.ConfirmButtonText)] = "Update"
        };
        var dialog = await DialogService.ShowAsync<DeductionDialog>("Edit Deduction", parameters, SmallEscDialog);
        var result = await dialog.Result;
        if (result is not { Canceled: false, Data: DeductionDto dto }) return;

        var resp = await PaymentService.UpdateDeductionAsync(UserId, record.Id, dto);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Deduction updated.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to update deduction.", Severity.Error);
    }

    private async Task DeleteDeductionAsync(DeductionDto record)
    {
        if (await DialogService.ShowMessageBox("Delete Deduction",
            $"Delete the {record.DeductionType} deduction ({FormatMoney(record.Amount, record.Currency)} / {record.Period})?",
            yesText: "Delete", cancelText: "Cancel") != true) return;

        var resp = await PaymentService.DeleteDeductionAsync(UserId, record.Id);
        if (resp.IsSuccessStatusCode) { await LoadAsync(); Snackbar.Add("Deduction deleted.", Severity.Success); }
        else Snackbar.Add(await ApiServiceBase.TryReadProblemDetailAsync(resp) ?? "Failed to delete deduction.", Severity.Error);
    }
}
