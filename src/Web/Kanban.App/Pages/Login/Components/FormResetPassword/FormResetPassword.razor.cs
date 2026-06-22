using Kanban.Adapter.Exceptions;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Pages.Login.Components.FormResetPassword;

public partial class FormResetPassword
{
    [Parameter] public EventCallback<List<string>> OnFeedbacksChanged { get; set; }
    [Parameter] public EventCallback OnBackToLogin { get; set; }

    private enum ResetStep { Email, Code, NewPassword, Success }

    private ResetStep _step = ResetStep.Email;

    private string _email = string.Empty;
    private string _code = string.Empty;
    private string _newPassword = string.Empty;
    private string _confirmPassword = string.Empty;
    private ValidateResetCodeResponse? _validateResetResponse;
    private bool _showPwd;
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];

    private bool CanSubmitEmail => !string.IsNullOrWhiteSpace(value: _email);
    private bool CanSubmitCode => _code.Trim().Length == 6;
    private bool CanSubmitNewPassword => !string.IsNullOrEmpty(value: _newPassword) && !string.IsNullOrEmpty(value: _confirmPassword);

    private async Task HandleSendEmail()
    {
        _listFeedbacks.Clear();
        _isSubmitting = true;
        StateHasChanged();

        try
        {
            await UserServiceApi.ForgotPassword(request: new ForgotPasswordRequest
            {
                Email = _email
            });
            _step = ResetStep.Code;
        }
        catch
        {
            _listFeedbacks = [LoginLocalizer[name: "RESET_SEND_ERROR"]];
        }

        _isSubmitting = false;
        await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
    }

    private async Task HandleVerifyCode()
    {
        _listFeedbacks.Clear();
        _isSubmitting = true;
        StateHasChanged();

        try
        {
            _validateResetResponse = await UserServiceApi.ValidateResetCode(request: new ValidateResetCodeRequest
            {
                Code = _code.Trim(),
                Email = _email
            });

            if (_validateResetResponse != null)
            {
                _step = ResetStep.NewPassword;
            }
        }
        catch
        {
            _listFeedbacks = [LoginLocalizer[name: "RESET_VERIFY_ERROR"]];
        }

        _isSubmitting = false;
        await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
    }

    private async Task HandleResetPassword()
    {
        _listFeedbacks.Clear();

        if (_newPassword != _confirmPassword)
        {
            _listFeedbacks = [LoginLocalizer[name: "RESET_PASSWORDS_MISMATCH"]];
            await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
            return;
        }

        _isSubmitting = true;
        StateHasChanged();

        try
        {
            await UserServiceApi.ResetPassword(request: new ResetPasswordRequest
            {
                TokenResetPassword = _validateResetResponse!.TokenResetPassword,
                NewPassword = _newPassword
            });
            _step = ResetStep.Success;
        }
        catch (ApiException ex) when (ex.ErrorMessages.Count > 0)
        {
            _listFeedbacks = ex.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [LoginLocalizer[name: "RESET_GENERIC_ERROR"]];
        }

        _isSubmitting = false;
        await OnFeedbacksChanged.InvokeAsync(arg: _listFeedbacks);
    }

    private async Task HandleBackToLogin() => await OnBackToLogin.InvokeAsync();
}
