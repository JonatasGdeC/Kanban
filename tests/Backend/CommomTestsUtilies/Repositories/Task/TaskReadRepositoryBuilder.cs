using Kanban.Domain.Entities;
using Kanban.Domain.Repositories.Task;
using Moq;

namespace CommomTestsUtilies.Repositories.Task;

public class TaskReadRepositoryBuilder
{
    private readonly Mock<ITaskReadRepository> _repositoryMock;

    public TaskReadRepositoryBuilder() => _repositoryMock = new Mock<ITaskReadRepository>();

    public TaskReadRepositoryBuilder GetAll(Guid columnId, Guid userId, List<TaskEntity>? tasks = null)
    {
        _repositoryMock.Setup(expression: repository => repository.GetAll(columnId, userId)).ReturnsAsync(value: tasks ?? []);
        return this;
    }

    public TaskReadRepositoryBuilder GetById(TaskEntity task)
    {
        _repositoryMock.Setup(expression: repository => repository.GetById(task.Id, It.IsAny<Guid>())).ReturnsAsync(value: task);
        return this;
    }

    public TaskReadRepositoryBuilder ExistsTaskInPosition(bool exists = false)
    {
        _repositoryMock
            .Setup(expression: repository => repository.ExistsTaskInPosition(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<Guid>()))
            .ReturnsAsync(value: exists);
        return this;
    }

    public ITaskReadRepository Build() => _repositoryMock.Object;
}
