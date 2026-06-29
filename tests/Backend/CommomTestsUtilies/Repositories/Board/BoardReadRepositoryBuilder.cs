using Kanban.Domain.Repositories.Boad;
using Moq;

namespace CommomTestsUtilies.Repositories.Board;

public class BoardReadRepositoryBuilder
{
    private readonly Mock<IBoardReadRepository> _repositoryMock;
    
    public BoardReadRepositoryBuilder() => _repositoryMock = new Mock<IBoardReadRepository>();
    
    
    public IBoardReadRepository Build() => _repositoryMock.Object;
}