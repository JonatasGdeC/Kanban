using Bogus;
using Kanban.Domain.Entities;

namespace CommomTestsUtilies.Entities;

public static class SubTaskBuilder
{
    public static SubTask Build(Guid? taskId = null, Guid? userId = null)
    {
        TaskEntity task = TaskEntityBuilder.Build(userId: userId);

        return new Faker<SubTask>()
            .RuleFor(property: s => s.Id, value: Guid.NewGuid())
            .RuleFor(property: s => s.Name, setter: faker => faker.Lorem.Word())
            .RuleFor(property: s => s.IsDone, setter: faker => faker.Random.Bool())
            .RuleFor(property: s => s.TaskId, value: taskId ?? task.Id)
            .RuleFor(property: s => s.Task, value: task);
    }
}
