using Bogus;
using Kanban.Communication.Requests.Board;

namespace CommomTestsUtilies.Requests;

public static class RegisterBoardRequestBuilder
{
    public static RegisterBoardRequest Build()
    {
        Faker faker = new();

        return new RegisterBoardRequest
        {
            Name = faker.Random.String(minLength: 3, maxLength: 200),
        };
    }
}