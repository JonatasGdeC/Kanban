using Kanban.Domain.Entities;
using Kanban.Domain.Security.Tokens;
using Moq;

namespace CommomTestsUtilies.Token;

public static class AccessTokenGenerator
{
    public static IAccessTokenGenerator Build()
    {
        Mock<IAccessTokenGenerator> mock = new();
        mock.Setup(expression: config => config.Generate(It.IsAny<User>())).Returns(value: "token");
        return mock.Object;
    }
}