using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.Action.Board.GetAll;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Layout.NavMenu;

public partial class NavMenu : FluxorComponent
{
    [Parameter] public bool Dark { get; set; }
    [Parameter] public EventCallback<bool> DarkOnChanged { get; set; }

    private bool _sidebarVisible = true;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(action: new GetAllBoardsAction());
    }

    private void NavigateToBoard(Guid boardId)
        => NavigationManager.NavigateTo(uri: $"/{boardId}");

    private bool BoardIsActive(Guid boardId)
        => NavigationManager.Uri.Contains(value: boardId.ToString());


    private async Task HandleLogout()
    {
        await CookieAuth.RemoveTokenAsync();
        NavigationManager.NavigateTo(uri: "/", forceLoad: true);
    }

    private void ToggleSidebar() 
        => _sidebarVisible = !_sidebarVisible;

    private async Task ToggleDark() 
        => await DarkOnChanged.InvokeAsync(arg: Dark = !Dark);
}
