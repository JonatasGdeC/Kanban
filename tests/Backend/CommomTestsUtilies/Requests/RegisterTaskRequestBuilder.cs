using Bogus;
using Kanban.Communication.Requests.Task;

namespace CommomTestsUtilies.Requests;

public static class RegisterTaskRequestBuilder
{
    public static RegisterTaskRequest Build()
    {
        Faker faker = new();

        return new RegisterTaskRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200),
            Description = faker.Lorem.Sentence()
        };
    }
}
