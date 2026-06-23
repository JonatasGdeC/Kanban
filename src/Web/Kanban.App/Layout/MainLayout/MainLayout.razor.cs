using Kanban.Communication.Dtos;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kanban.App.Layout.MainLayout;

public partial class MainLayout
{
    private BoardDto? _board;
    private bool _isAuthenticated;
    bool _dark;
    bool _mobileSidebar;
    
    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _isAuthenticated = authState.User.Identity?.IsAuthenticated == true;

        AuthenticationStateProvider.AuthenticationStateChanged += async (task) =>
        {
            AuthenticationState state = await task;
            _isAuthenticated = state.User.Identity?.IsAuthenticated == true;
            StateHasChanged();
        };

        ModalUseState.OnChange += SetCurrentBoardByModalUseState;
        BoardUseState.OnChange += SetCurrentBoardByModalUseState;
    }
    
    private void SetCurrentBoardByModalUseState() => SetCurrentBoard(board: ModalUseState.Board);
    
    private void SetCurrentBoard(BoardDto? board)
    {
        _board = board;
        StateHasChanged();
    }

    public void Dispose()
    {
        BoardUseState.OnChange -= SetCurrentBoardByModalUseState;
    }
}