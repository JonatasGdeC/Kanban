using Bogus;
using Kanban.Domain.Entities;

namespace CommomTestsUtilies.Entities;

public static class TaskEntityBuilder
{
    public static TaskEntity Build(Guid? columnId = null, Guid? userId = null)
    {
        Column column = ColumnBuilder.Build(userId: userId);

        return new Faker<TaskEntity>()
            .RuleFor(property: t => t.Id, value: Guid.NewGuid())
            .RuleFor(property: t => t.Name, setter: faker => faker.Lorem.Word())
            .RuleFor(property: t => t.Description, setter: faker => faker.Lorem.Sentence())
            .RuleFor(property: t => t.Order, setter: faker => faker.Random.Int(min: 1, max: 10))
            .RuleFor(property: t => t.SubTasks, value: [])
            .RuleFor(property: t => t.ColumnId, value: columnId ?? column.Id)
            .RuleFor(property: t => t.Column, value: column);
    }
}
