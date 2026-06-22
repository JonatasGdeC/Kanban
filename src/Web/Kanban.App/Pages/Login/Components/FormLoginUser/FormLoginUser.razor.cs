using Kanban.Adapter.Exceptions;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Pages.Login.Components.FormLoginUser;

public partial class FormLoginUser
{
    [Parameter] public EventCallback<List<string>> OnFeedbacksChanged { get; set; }
    
    private readonly LoginRequest _loginRequest = new()
    {
        Email = string.Empty,
        Password = string.Empty
    };
    
    private bool _showPwd;
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    
    private bool CanSubmit => !string.IsNullOrEmpty(value: _loginRequest.Email) &&
                              !string.IsNullOrEmpty(value: _loginRequest.Password);
    
    private async Task HandleLoginAsync()
    {
        _listFeedbacks.Clear();
        _isSubmitting = true;

        try
        {
            LoginResponse response = await UserServiceApi.Login(request: _loginRequest);
            await CookieAuthenticationStateProvider.SetTokenAsync(token: response.Token);
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [LoginLocalizer[name: "LOGIN_ERROR_GENERIC"]];
        }

        _isSubmitting = false;
        await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
    }
}