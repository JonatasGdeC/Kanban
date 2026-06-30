using Bogus;
using Kanban.Domain.Entities;

namespace CommomTestsUtilies.Entities;

public static class ColumnBuilder
{
    public static Column Build(Guid? boardId = null, Guid? userId = null)
    {
        Board board = BoardBuilder.Build(userId: userId);

        return new Faker<Column>()
            .RuleFor(property: c => c.Id, value: Guid.NewGuid())
            .RuleFor(property: c => c.Name, setter: faker => faker.Lorem.Word())
            .RuleFor(property: c => c.Color, setter: faker => faker.Internet.Color())
            .RuleFor(property: c => c.Order, setter: faker => faker.Random.Int(min: 1, max: 10))
            .RuleFor(property: c => c.Tasks, value: [])
            .RuleFor(property: c => c.BoardId, value: boardId ?? board.Id)
            .RuleFor(property: c => c.Board, value: board);
    }
}
