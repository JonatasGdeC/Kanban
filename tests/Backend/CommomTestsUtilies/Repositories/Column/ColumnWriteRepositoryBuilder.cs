using Kanban.Domain.Repositories.Column;
using Moq;

namespace CommomTestsUtilies.Repositories.Column;
using Kanban.Domain.Entities;

public class ColumnWriteRepositoryBuilder
{
    private readonly Mock<IColumWriteRepository> _repositoryMock;

    public ColumnWriteRepositoryBuilder() => _repositoryMock = new Mock<IColumWriteRepository>();

    public ColumnWriteRepositoryBuilder GetById(Column column)
    {
        _repositoryMock
            .Setup(expression: repository => repository.GetById(column.Id, It.IsAny<Guid>()))
            .ReturnsAsync(value: column);
        return this;
    }

    public IColumWriteRepository Build() => _repositoryMock.Object;
}
