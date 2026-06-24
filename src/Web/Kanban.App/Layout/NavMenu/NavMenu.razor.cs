using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.Board.Action;
using Kanban.Communication.Responses.Board;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Layout.NavMenu;

public partial class NavMenu : FluxorComponent
{
    [Parameter] public bool Dark { get; set; }
    [Parameter] public EventCallback<bool> DarkOnChanged { get; set; }

    private bool _sidebarVisible = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Dispatcher.Dispatch(action: new GetAllBoardsAction());

        try
        {
            GetAllBoardsResponse? response = await BoardServiceApi.GetAll();
            if (response != null)
            {
                Dispatcher.Dispatch(action: new GetAllBoardsSuccessAction(Boards: response.ListBoards));
            }
        }
        catch { /* ignored */ }
    }

    private void NavigateToBoard(Guid boardId)
        => NavigationManager.NavigateTo(uri: $"/{boardId}");

    private bool BoardIsActive(Guid boardId)
        => NavigationManager.Uri.Contains(value: boardId.ToString());


    private async Task HandleLogout()
    {
        await CookieAuth.RemoveTokenAsync();
        NavigationManager.NavigateTo(uri: "/");
    }

    private void ToggleSidebar() 
        => _sidebarVisible = !_sidebarVisible;

    private async Task ToggleDark() 
        => await DarkOnChanged.InvokeAsync(arg: Dark = !Dark);
}
