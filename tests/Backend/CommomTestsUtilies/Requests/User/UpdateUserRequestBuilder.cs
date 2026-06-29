using Bogus;
using Kanban.Communication.Requests.User;

namespace CommomTestsUtilies.Requests.User;

public static class UpdateUserRequestBuilder
{
    public static UpdateUserRequest Build()
    {
        Faker faker = new();
        
        return new UpdateUserRequest
        {
            Name = faker.Person.FullName,
            Email = faker.Internet.Email()
        };
    }
}