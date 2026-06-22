using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Board;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Kanban.App.Layout.NavMenu;

public partial class NavMenu : IDisposable
{
    [Parameter] public bool Dark { get; set; }
    [Parameter] public EventCallback<bool> DarkOnChanged { get; set; }
    [Parameter] public EventCallback<BoardDto?> SetCurrentBoard { get; set; }
    
    private bool _sidebarVisible = true;
    private bool _isLoading = true;
    
    protected override async Task OnInitializedAsync()
    {
        BoardUseState.OnChange += StateHasChanged;
        
        if (!BoardUseState.List().Any())
        {
            try
            {
                GetAllBoardsResponse? response = await BoardServiceApi.GetAll();
                if (response != null)
                {
                    BoardUseState.Set(boards: response.ListBoards);
                }
            }
            catch { /* silently ignore load errors */ }
            finally
            {
                NavigationManager.LocationChanged += HandleLocationChanged;
                _isLoading = false;
            }
        }
    }
    
    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e) => StateHasChanged();

    private async Task NavigateToBoard(Guid boardId)
    {
        NavigationManager.NavigateTo(uri: $"/{boardId}");
        await HandleSetCurrentBoard(boardId: boardId);
    }

    private bool BoardIsActive(Guid boardId) => NavigationManager.Uri.Contains(value: boardId.ToString());


    private async Task HandleLogout()
    {
        await CookieAuth.RemoveTokenAsync();
        NavigationManager.NavigateTo(uri: "/", forceLoad: true);
    }

    private void ToggleSidebar() => _sidebarVisible = !_sidebarVisible;

    private async Task ToggleDark() => await DarkOnChanged.InvokeAsync(arg: Dark = !Dark);
    
    private async Task HandleSetCurrentBoard(Guid boardId)
    {
        BoardDto board = BoardUseState.List().First(predicate: board => board.Id == boardId);
        await SetCurrentBoard.InvokeAsync(arg: board);
    }

    public void Dispose()
    {
        BoardUseState.OnChange -= StateHasChanged;
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
}
