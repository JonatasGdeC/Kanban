using AutoMapper;
using CommomTestsUtilies.Cryptography;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories.User;
using CommomTestsUtilies.Requests.User;
using CommomTestsUtilies.Token;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.User.Login;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.User;
using Kanban.Domain.Entities;

public class LoginUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        User user = UserBuilder.Build();
        LoginRequest request = LoginRequestBuilder.Build();
        request.Email = user.Email;
        LoginUseCase useCase = CreateUseCase(user: user, password: request.Password);
        
        LoginResponse result = await useCase.Execute(request: request);

        result.Should().NotBeNull();
        result.User.Name.Should().Be(expected: user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        User user = UserBuilder.Build();
        LoginRequest request = LoginRequestBuilder.Build();
        LoginUseCase useCase = CreateUseCase(user: user, password: request.Password);

        Func<Task<LoginResponse>> act = async () => await useCase.Execute(request: request);

        ExceptionAssertions<InvalidLoginException>? result = await act.Should().ThrowAsync<InvalidLoginException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.INVALID_LOGIN));
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        User user = UserBuilder.Build();
        LoginRequest request = LoginRequestBuilder.Build();
        request.Email = user.Email;
        LoginUseCase useCase = CreateUseCase(user: user);
    
        Func<Task<LoginResponse>> act = async () => await useCase.Execute(request: request);
    
        ExceptionAssertions<InvalidLoginException>? result = await act.Should().ThrowAsync<InvalidLoginException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.INVALID_LOGIN));
    }
    
    private LoginUseCase CreateUseCase(User user, string? password = null)
    {
        IUserReadRepository readRepository = new UserReadRepositoryBuilder().GetByEmail(user: user).Build();
        IAccessTokenGenerator accessTokenGenerator = AccessTokenGenerator.Build();
        IEncrypter passwordEncrypter =  new PasswordEncrypterBuilder().Verify(password: password).Build();
        IMapper mapper = MapperBuilder.Build();
        
        return new LoginUseCase(
            readRepository: readRepository, 
            passwordEncrypter: passwordEncrypter,
            tokenGenerator: accessTokenGenerator, 
            mapper: mapper);
    }
}