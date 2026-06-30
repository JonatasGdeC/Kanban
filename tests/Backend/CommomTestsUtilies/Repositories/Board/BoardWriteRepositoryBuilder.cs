using Kanban.Domain.Repositories.Boad;
using Moq;

namespace CommomTestsUtilies.Repositories.Board;
using Kanban.Domain.Entities;

public class BoardWriteRepositoryBuilder
{
    private readonly Mock<IBoardWriteRepository> _repositoryMock;
    
    public BoardWriteRepositoryBuilder() => _repositoryMock = new Mock<IBoardWriteRepository>();
    
    public BoardWriteRepositoryBuilder GetByTitle(Board board)
    {
        _repositoryMock.Setup(expression: repository => repository.GetByTitle(board.Name, board.UserId, ignoreBoardId: It.IsAny<Guid?>())).ReturnsAsync(value: board);
        return this;
    }
    
    public BoardWriteRepositoryBuilder GetById(Board board)
    {
        _repositoryMock.Setup(expression: repository => repository.GetById(board.Id, userId: board.UserId)).ReturnsAsync(value: board);
        return this;
    }
    
    
    public IBoardWriteRepository Build() => _repositoryMock.Object;
}