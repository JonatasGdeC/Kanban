using Kanban.App.Services.SnackbarService;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Components.AddSnackbar;

public partial class AddSnackbar : ComponentBase, IDisposable
{
    protected override void OnInitialized()
    {
        SnackbarService.OnChange += HandleChange;
    }

    private void HandleChange()
    {
        InvokeAsync(workItem: StateHasChanged);
        foreach (SnackbarItem item in SnackbarService.Items.Where(predicate: x => !x.IsLeaving))
        {
            ScheduleRemoval(item: item);
        }
    }

    private void ScheduleRemoval(SnackbarItem item)
    {
        _ = Task.Delay(millisecondsDelay: item.DurationMs).ContinueWith(continuationFunction: _ =>
        {
            item.IsLeaving = true;
            InvokeAsync(workItem: StateHasChanged);

            return Task.Delay(millisecondsDelay: 300).ContinueWith(continuationAction: __ =>
            {
                SnackbarService.Remove(id: item.Id);
            });
        });
    }

    private void Dismiss(SnackbarItem item)
    {
        item.IsLeaving = true;
        StateHasChanged();
        _ = Task.Delay(millisecondsDelay: 300).ContinueWith(continuationAction: _ => SnackbarService.Remove(id: item.Id));
    }

    private static string GetIcon(SnackbarSeverity severity) => severity switch
    {
        SnackbarSeverity.Success => "✓",
        SnackbarSeverity.Error   => "✕",
        SnackbarSeverity.Warning => "⚠",
        SnackbarSeverity.Info    => "ℹ",
        _                        => "●"
    };

    public void Dispose()
    {
        SnackbarService.OnChange -= HandleChange;
    }
}
