using Microsoft.AspNetCore.Components.Authorization;

namespace Kanban.App.Layout.MainLayout;

public partial class MainLayout
{
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
    }
}