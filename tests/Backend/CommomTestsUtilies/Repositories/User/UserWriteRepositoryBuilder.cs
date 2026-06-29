using Kanban.Domain.Repositories.User;
using Moq;

namespace CommomTestsUtilies.Repositories.User;
using Kanban.Domain.Entities;

public class UserWriteRepositoryBuilder
{
    private readonly Mock<IUserWriteRepository> _repositoryMock;

    public UserWriteRepositoryBuilder() => _repositoryMock = new Mock<IUserWriteRepository>();
    
    public UserWriteRepositoryBuilder Add(User user)
    {
        _repositoryMock.Setup(expression: repository => repository.Add(user: user));
        return this;
    }
    
    public UserWriteRepositoryBuilder Update(User user)
    {
        _repositoryMock.Setup(expression: repository => repository.Update(user: user));
        return this;
    }
    
    public UserWriteRepositoryBuilder Delete(User user)
    {
        _repositoryMock.Setup(expression: repository => repository.Delete(user: user));
        return this;
    }

    public UserWriteRepositoryBuilder GetById(User user)
    {
        _repositoryMock.Setup(expression: repository => repository.GetById(user.Id)).ReturnsAsync(value: user);
        return this;
    }
    
    public IUserWriteRepository Build() => _repositoryMock.Object;
}