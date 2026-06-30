using Bogus;
using Kanban.Communication.Requests.SubTask;

namespace CommomTestsUtilies.Requests;

public static class UpdateSubTaskRequestBuilder
{
    public static UpdateSubTaskRequest Build()
    {
        Faker faker = new();

        return new UpdateSubTaskRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200),
            IsDone = faker.Random.Bool()
        };
    }
}
