namespace DZDDashboard.Client.Components.Pages.Employees;

public sealed class SectionEditState<T> where T : class
{
    public bool IsEditing { get; private set; }

    private T? _backup;

    public void Begin(T snapshot)
    {
        _backup   = snapshot;
        IsEditing = true;
    }

    public T? Cancel()
    {
        IsEditing = false;
        var b = _backup;
        _backup   = null;
        return b;
    }

    public void Commit()
    {
        IsEditing = false;
        _backup   = null;
    }
}
