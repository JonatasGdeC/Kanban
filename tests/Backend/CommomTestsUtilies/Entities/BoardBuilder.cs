using Bogus;
using Kanban.Domain.Entities;

namespace CommomTestsUtilies.Entities;

public static class BoardBuilder
{
    public static Board Build(string? boardName = null, Guid? userId = null)
    {
        return new Faker<Board>()
            .RuleFor(property: b => b.Id, value: Guid.NewGuid())
            .RuleFor(property: b => b.Name, setter: faker => boardName ?? faker.Lorem.Word())
            .RuleFor(property: b => b.Columns, value: [])
            .RuleFor(property: b => b.UserId, value: userId ?? Guid.NewGuid());
    }
}