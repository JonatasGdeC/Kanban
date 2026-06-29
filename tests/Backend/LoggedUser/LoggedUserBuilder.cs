using CashFlow.Domain.Enitites;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommomTestsUtilies.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        Mock<ILoggedUser> mock = new();
        mock.Setup(expression: loggedUser => loggedUser.Get()).ReturnsAsync(value: user);
        return mock.Object;
    }
}