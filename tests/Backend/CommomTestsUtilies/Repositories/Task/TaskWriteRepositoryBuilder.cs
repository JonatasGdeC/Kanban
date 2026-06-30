using Kanban.Domain.Repositories.Task;
using Moq;

namespace CommomTestsUtilies.Repositories.Task;
using Kanban.Domain.Entities;

public class TaskWriteRepositoryBuilder
{
    private readonly Mock<ITaskWriteRepository> _repositoryMock;

    public TaskWriteRepositoryBuilder() => _repositoryMock = new Mock<ITaskWriteRepository>();

    public TaskWriteRepositoryBuilder GetById(TaskEntity task)
    {
        _repositoryMock
            .Setup(expression: repository => repository.GetById(task.Id, It.IsAny<Guid>()))
            .ReturnsAsync(value: task);
        return this;
    }

    public ITaskWriteRepository Build() => _repositoryMock.Object;
}
