using DZDDashboard.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DZDDashboard.Client.Components.Shared;

public abstract class CrudDialogBase : ComponentBase
{
    [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected DZDDashboard.Client.Localization.AppLocalizer Loc { get; set; } = default!;

    protected bool _saving;

    protected void Cancel() => MudDialog.Cancel();

    protected async Task SaveAsync(
        Func<Task<HttpResponseMessage>> operation,
        string entityName,
        Func<bool>? validate = null)
    {
        if (validate != null && !validate())
            return;

        _saving = true;
        try
        {
            var response = await operation();

            if (response.IsSuccessStatusCode)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                var detail = await ApiServiceBase.TryReadProblemDetailAsync(response);
                Snackbar.Add(detail ?? string.Format(Loc["crud.saveFailed"], entityName), Severity.Error);
            }
        }
        catch (Exception)
        {
            Snackbar.Add(string.Format(Loc["crud.unexpectedSaveError"], entityName), Severity.Error);
        }
        finally
        {
            _saving = false;
        }
    }

    protected async Task SaveAsync(
        int entityId,
        Func<Task<HttpResponseMessage>> createOperation,
        Func<Task<HttpResponseMessage>> updateOperation,
        string entityName,
        Func<bool>? validate = null)
    {
        var operation = entityId == 0 ? createOperation : updateOperation;
        await SaveAsync(operation, entityName, validate);
    }
}
