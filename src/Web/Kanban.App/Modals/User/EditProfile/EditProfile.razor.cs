using Kanban.Adapter.Exceptions;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.User;

namespace Kanban.App.Modals.User.EditProfile;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

public partial class EditProfile
{
    private readonly UpdateUserRequest _profileRequest = new() { Name = string.Empty, Email = string.Empty };
    private readonly UpdatePasswordRequest _passwordRequest = new() { OldPassword = string.Empty, NewPassword = string.Empty };
    private string _confirmNewPassword = string.Empty;

    private bool _isSavingProfile;
    private bool _isSavingPassword;
    private bool _isDeleting;
    private bool _profileSaved;
    private bool _passwordSaved;
    private bool _showOld;
    private bool _showNew;
    private bool _confirmDelete;
    private List<string> _profileFeedbacks = [];
    private List<string> _passwordFeedbacks = [];
    private List<string> _deleteFeedbacks = [];

    protected override async Task OnInitializedAsync()
    {
        UserDto? loggerUser = await UserServiceApi.Get();
        _profileRequest.Name  = loggerUser?.Name  ?? string.Empty;
        _profileRequest.Email = loggerUser?.Email ?? string.Empty;
    }

    private async Task HandleUpdateProfile()
    {
        _isSavingProfile = true;
        _profileFeedbacks = [];
        _profileSaved = false;

        try
        {
            await UserServiceApi.Update(request: _profileRequest);
            _profileSaved = true;
        }
        catch (ApiException ex) when (ex.ErrorMessages.Count > 0)
        {
            _profileFeedbacks = ex.ErrorMessages.ToList();
        }
        catch
        {
            _profileFeedbacks = [ModalLocalizer[name: "PROFILE_ERROR"]];
        }
        finally
        {
            _isSavingProfile = false;
        }
    }

    private async Task HandleDeleteAccount()
    {
        _isDeleting = true;
        _deleteFeedbacks = [];

        try
        {
            await UserServiceApi.Delete();
            await CookieAuth.RemoveTokenAsync();
            NavigationManager.NavigateTo(uri: "/", forceLoad: true);
        }
        catch (ApiException ex) when (ex.ErrorMessages.Count > 0)
        {
            _deleteFeedbacks = ex.ErrorMessages.ToList();
        }
        catch
        {
            _deleteFeedbacks = [ModalLocalizer[name: "DELETE_ACCOUNT_ERROR"]];
        }
        finally
        {
            _isDeleting = false;
        }
    }

    private async Task HandleUpdatePassword()
    {
        _isSavingPassword = true;
        _passwordFeedbacks = [];
        _passwordSaved = false;

        if (_passwordRequest.NewPassword != _confirmNewPassword)
        {
            _passwordFeedbacks = [ModalLocalizer[name: "PASSWORD_MISMATCH"]];
            _isSavingPassword = false;
            return;
        }

        try
        {
            await UserServiceApi.UpdatePassword(request: _passwordRequest);
            _passwordSaved = true;
            _passwordRequest.OldPassword = string.Empty;
            _passwordRequest.NewPassword = string.Empty;
            _confirmNewPassword = string.Empty;
        }
        catch (ApiException ex) when (ex.ErrorMessages.Count > 0)
        {
            _passwordFeedbacks = ex.ErrorMessages.ToList();
        }
        catch
        {
            _passwordFeedbacks = [ModalLocalizer[name: "PASSWORD_UPDATE_ERROR"]];
        }
        finally
        {
            _isSavingPassword = false;
        }
    }
}
