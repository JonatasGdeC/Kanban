using Bogus;
using Kanban.Communication.Requests.Task;

namespace CommomTestsUtilies.Requests;

public static class UpdateTaskRequestBuilder
{
    public static UpdateTaskRequest Build(Guid? columnId = null)
    {
        Faker faker = new();

        return new UpdateTaskRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200),
            Description = faker.Lorem.Sentence(),
            Order = faker.Random.Int(min: 1, max: 10),
            ColumnId = columnId ?? Guid.NewGuid()
        };
    }
}
