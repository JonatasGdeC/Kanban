using Kanban.Domain.Repositories.SubTask;
using Moq;

namespace CommomTestsUtilies.Repositories.SubTask;
using Kanban.Domain.Entities;

public class SubTaskWriteRepositoryBuilder
{
    private readonly Mock<ISubTaskWriteRepository> _repositoryMock;

    public SubTaskWriteRepositoryBuilder() => _repositoryMock = new Mock<ISubTaskWriteRepository>();

    public SubTaskWriteRepositoryBuilder GetById(SubTask subTask)
    {
        _repositoryMock.Setup(expression: repository => repository.GetById(subTask.Id, It.IsAny<Guid>())).ReturnsAsync(value: subTask);
        return this;
    }

    public ISubTaskWriteRepository Build() => _repositoryMock.Object;
}
