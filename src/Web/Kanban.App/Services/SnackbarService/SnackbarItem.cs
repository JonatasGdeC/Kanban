namespace Kanban.App.Services.SnackbarService;

public sealed class SnackbarItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Message { get; init; } = string.Empty;
    public SnackbarSeverity Severity { get; init; }
    public int DurationMs { get; init; }
    public bool IsLeaving { get; set; }
}
