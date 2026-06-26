using Bogus;
using Kanban.Communication.Requests.Column;

namespace CommomTestsUtilies.Requests;

public static class RegisterColumnRequestBuilder
{
    public static RegisterColumnRequest Build()
    {
        Faker faker = new();

        return new RegisterColumnRequest
        {
            Name = faker.Random.String(minLength: 1, maxLength: 200),
            Color = faker.Internet.Color()
        };
    }
}