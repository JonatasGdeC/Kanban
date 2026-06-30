using Bogus;
using Kanban.Communication.Requests.SubTask;

namespace CommomTestsUtilies.Requests;

public static class RegisterSubTaskRequestBuilder
{
    public static RegisterSubTaskRequest Build()
    {
        Faker faker = new();

        return new RegisterSubTaskRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200)
        };
    }
}
