using Kanban.Domain.Repositories.User;
using Moq;

namespace CommomTestsUtilies.Repositories.User;
using Kanban.Domain.Entities;

public class UserReadRepositoryBuilder
{
    private readonly Mock<IUserReadRepository> _repositoryMock;
    
    public UserReadRepositoryBuilder() => _repositoryMock = new Mock<IUserReadRepository>();

    public UserReadRepositoryBuilder GetByEmail(User user)
    {
        _repositoryMock.Setup(expression: repository => repository.GetByEmail(user.Email)).ReturnsAsync(value: user);
        return this;
    }
    
    public IUserReadRepository Build() => _repositoryMock.Object;
}