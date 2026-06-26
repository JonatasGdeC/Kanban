using Bogus;
using Kanban.Communication.Requests.User;

namespace CommomTestsUtilies.Requests.User;

public static class RegisterUserRequestBuilder
{
    public static RegisterUserRequest Build()
    {
        Faker faker = new();

        return new RegisterUserRequest
        {
            Name = faker.Person.FullName,
            Email = faker.Internet.Email(),
            Password = faker.Internet.Password(prefix: "!Aa1"),
        };
    }
}