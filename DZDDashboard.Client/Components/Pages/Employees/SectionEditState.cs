namespace DZDDashboard.Client.Components.Pages.Employees;

/// <summary>
/// Encapsulates the IsEditing flag and a snapshot backup for a single editable form section.
/// Replaces the repetitive (_editingXxx, _xxxBackup) field pair pattern.
/// </summary>
public sealed class SectionEditState<T> where T : class
{
    public bool IsEditing { get; private set; }

    private T? _backup;

    /// <summary>Opens edit mode and stores <paramref name="snapshot"/> for potential rollback.</summary>
    public void Begin(T snapshot)
    {
        _backup   = snapshot;
        IsEditing = true;
    }

    /// <summary>Closes edit mode and returns the stored snapshot (null if never opened).</summary>
    public T? Cancel()
    {
        IsEditing = false;
        var b = _backup;
        _backup   = null;
        return b;
    }

    /// <summary>Closes edit mode and discards the backup (call after a successful save).</summary>
    public void Commit()
    {
        IsEditing = false;
        _backup   = null;
    }
}
