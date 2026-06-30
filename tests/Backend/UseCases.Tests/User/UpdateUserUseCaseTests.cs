using CommomTestsUtilies.Entities;
using CommomTestsUtilies.LoggedUser;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.User;
using CommomTestsUtilies.Requests.User;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.User.Update;
using Kanban.Communication.Requests.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.User;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        UpdateUserUseCase useCase = CreateUseCase(user: user);
        UpdateUserRequest request = UpdateUserRequestBuilder.Build();
        
        Func<Task> act = async () => await useCase.Execute(request: request);

       await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        User user = UserBuilder.Build();
        UpdateUserRequest request = UpdateUserRequestBuilder.Build(email: user.Email);
        UpdateUserUseCase useCase = CreateUseCase(user: user, email: request.Email);
        
        Func<Task> act = async () => await useCase.Execute(request: request);
    
        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_ALREADY_EXISTS));
    }

    
    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        IUserWriteRepository whiteRepository = new UserWriteRepositoryBuilder().Build();
        UserReadRepositoryBuilder readRepository = new();
        ILoggedUser loggedUser = LoggedUserBuilder.Build(user: email == null ? user : UserBuilder.Build());
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

        if (!string.IsNullOrEmpty(value: email))
        {
            readRepository.GetByEmail(user: user);
        }
        
        return new UpdateUserUseCase(
            writeRepository: whiteRepository, 
            readRepository: readRepository.Build(), 
            loggedUser: loggedUser, 
            unitOfWork: unitOfWork);
    }
}