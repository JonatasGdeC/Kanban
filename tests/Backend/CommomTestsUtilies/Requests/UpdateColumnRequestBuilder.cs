using Bogus;
using Kanban.Communication.Requests.Column;

namespace CommomTestsUtilies.Requests;

public static class UpdateColumnRequestBuilder
{
    public static UpdateColumnRequest Build()
    {
        Faker faker = new();

        return new UpdateColumnRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200),
            Color = faker.Internet.Color(),
            Order = faker.Random.Int(min: 1, max: 10)
        };
    }
}
