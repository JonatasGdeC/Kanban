using Kanban.Domain.Repositories.Boad;
using Moq;

namespace CommomTestsUtilies.Repositories.Board;
using Kanban.Domain.Entities;

public class BoardReadRepositoryBuilder
{
    private readonly Mock<IBoardReadRepository> _repositoryMock;

    public BoardReadRepositoryBuilder() => _repositoryMock = new Mock<IBoardReadRepository>();

    public BoardReadRepositoryBuilder GetById(Board board)
    {
        _repositoryMock.Setup(expression: repository => repository.GetById(board.Id, board.UserId)).ReturnsAsync(value: board);
        return this;
    }

    public IBoardReadRepository Build() => _repositoryMock.Object;
}