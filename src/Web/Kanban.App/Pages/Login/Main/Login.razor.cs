namespace Kanban.App.Pages.Login.Main;

public partial class Login
{
    private enum AuthMode { Login, Register, Reset }

    private AuthMode _mode = AuthMode.Login;
    private List<string> _listFeedbacks = [];

    private string PageTitle => _mode switch
    {
        AuthMode.Register => LoginLocalizer[name: "REGISTER_PAGE_TITLE"],
        AuthMode.Reset    => LoginLocalizer[name: "RESET_PAGE_TITLE"],
        _                 => LoginLocalizer[name: "LOGIN_PAGE_TITLE"]
    };

    private string CardTitle => _mode switch
    {
        AuthMode.Register => LoginLocalizer[name: "REGISTER_CARD_TITLE"],
        AuthMode.Reset    => LoginLocalizer[name: "RESET_CARD_TITLE"],
        _                 => LoginLocalizer[name: "LOGIN_CARD_TITLE"]
    };

    private void GoToLogin()
    {
        _listFeedbacks.Clear();
        _mode = AuthMode.Login;
    }

    private void GoToRegister()
    {
        _listFeedbacks.Clear();
        _mode = AuthMode.Register;
    }

    private void GoToReset()
    {
        _listFeedbacks.Clear();
        _mode = AuthMode.Reset;
    }
}
