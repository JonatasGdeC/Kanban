using Kanban.Domain.Repositories.User;
using Moq;

namespace CommomTestsUtilies.Repositories.User;

public class UserWriteRepositoryBuilder
{
    private readonly Mock<IUserWriteRepository> _repositoryMock;

    public UserWriteRepositoryBuilder() => _repositoryMock = new Mock<IUserWriteRepository>();
    
    public IUserWriteRepository Build() => _repositoryMock.Object;
}