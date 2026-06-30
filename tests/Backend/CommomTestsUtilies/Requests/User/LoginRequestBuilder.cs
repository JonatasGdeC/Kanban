using Bogus;
using Kanban.Communication.Requests.User;

namespace CommomTestsUtilies.Requests.User;

public static class LoginRequestBuilder
{
    public static LoginRequest Build(string? email = null)
    {
        Faker faker = new();

        return new LoginRequest
        {
            Email = email ?? faker.Internet.Email(),
            Password = faker.Internet.Password(prefix: "!Aa1")
        };
    }
}