using Kanban.Domain.Repositories.Column;
using Moq;

namespace CommomTestsUtilies.Repositories.Column;
using Kanban.Domain.Entities;

public class ColumnReadRepositoryBuilder
{
    private readonly Mock<IColumnReadRepository> _repositoryMock;

    public ColumnReadRepositoryBuilder() => _repositoryMock = new Mock<IColumnReadRepository>();

    public ColumnReadRepositoryBuilder GetAll(Guid boardId, Guid userId, List<Column>? columns = null)
    {
        _repositoryMock.Setup(expression: repository => repository.GetAll(boardId, userId)).ReturnsAsync(value: columns ?? []);
        return this;
    }

    public IColumnReadRepository Build() => _repositoryMock.Object;
}
