using Kanban.Adapter.Exceptions;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Pages.Login.Components.FormRegisterUser;

public partial class FormRegisterUser
{
    [Parameter] public EventCallback<List<string>> OnFeedbacksChanged { get; set; }
    
    private readonly RegisterUserRequest _registerModel = new()
    {
        Name = string.Empty,
        Email = string.Empty,
        Password = string.Empty
    };

    private string _confirmPassword = string.Empty;
    private bool _showPwd;
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];

    private bool CanSubmit => !string.IsNullOrEmpty(value: _registerModel.Name) &&
                              !string.IsNullOrEmpty(value: _registerModel.Email) &&
                              !string.IsNullOrEmpty(value: _registerModel.Password) &&
                              !string.IsNullOrEmpty(value: _confirmPassword);

    private async Task HandleRegisterAsync()
    {
        _listFeedbacks.Clear();
        _isSubmitting = true;

        if (_registerModel.Password != _confirmPassword)
        {
            _listFeedbacks = [LoginLocalizer[name: "REGISTER_PASSWORD_MISMATCH"]];
            return;
        }
        
        try
        {
            RegisteredUserResponse response = await UserServiceApi.Register(request: _registerModel);
            await CookieAuthenticationStateProvider.SetTokenAsync(token: response.Token);
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [LoginLocalizer[name: "REGISTER_ERROR_GENERIC"]];
        }
        
        _isSubmitting = false;
        await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
    }
}