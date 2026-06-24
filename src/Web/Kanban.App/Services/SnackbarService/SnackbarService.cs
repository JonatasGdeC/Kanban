namespace Kanban.App.Services.SnackbarService;

public class SnackbarService
{
    public event Action? OnChange;

    private readonly List<SnackbarItem> _items = [];
    public IReadOnlyList<SnackbarItem> Items => _items;

    public void Show(string message, SnackbarSeverity severity = SnackbarSeverity.Normal, int durationMs = 4000)
    {
        _items.Add(item: new SnackbarItem { Message = message, Severity = severity, DurationMs = durationMs });
        OnChange?.Invoke();
    }

    public void Remove(Guid id)
    {
        _items.RemoveAll(match: x => x.Id == id);
        OnChange?.Invoke();
    }
}
