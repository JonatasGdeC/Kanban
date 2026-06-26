using Bogus;
using Kanban.Communication.Requests.User;

namespace CommomTestsUtilies.Requests.User;

public static class LoginRequestBuilder
{
    public static LoginRequest Build()
    {
        Faker faker = new();

        return new LoginRequest
        {
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(prefix: "!Aa1")
        };
    }
}