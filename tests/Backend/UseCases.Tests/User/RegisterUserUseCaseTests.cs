using AutoMapper;
using CommomTestsUtilies.Cryptography;
using CommomTestsUtilies.Entities;
using CommomTestsUtilies.Mapper;
using CommomTestsUtilies.Repositories;
using CommomTestsUtilies.Repositories.User;
using CommomTestsUtilies.Requests.User;
using CommomTestsUtilies.Token;
using FluentAssertions;
using FluentAssertions.Specialized;
using Kanban.Application.UseCase.User.Register;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace UseCases.Tests.User;
using Kanban.Domain.Entities;
using System.Threading.Tasks;

public class RegisterUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        RegisterUserUseCase useCase = CreateUseCase();
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();

        RegisteredUserResponse response = await useCase.Execute(request: request);
        
        response.Should().NotBeNull();
        response.User.Name.Should().Be(expected: request.Name);
        response.User.Email.Should().Be(expected: request.Email);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;
        RegisterUserUseCase useCase = CreateUseCase();
        
        Func<Task<RegisteredUserResponse>> act = async () => await useCase.Execute(request: request);

        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.NAME_IS_REQUIRED));
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        User user = UserBuilder.Build();
        user.Email = request.Email;
        RegisterUserUseCase useCase = CreateUseCase(user: user);
        
        Func<Task<RegisteredUserResponse>> act = async () => await useCase.Execute(request: request);
    
        ExceptionAssertions<ErrorOnValidationException>? result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(exceptionExpression: ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_ALREADY_EXISTS));
    }
    
    private RegisterUserUseCase CreateUseCase(User? user = null)
    {
        IUserWriteRepository userWriteRepository = new UserWriteRepositoryBuilder().Build();
        UserReadRepositoryBuilder userReadRepository = new();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        IEncrypter encrypter = new PasswordEncrypterBuilder().Build();
        IAccessTokenGenerator accessTokenGenerator = AccessTokenGenerator.Build();
        
        if (user != null)
        {
            userReadRepository.GetByEmail(user: user);
        }
        
        return new RegisterUserUseCase(
            writeRepository: userWriteRepository,
            readRepository: userReadRepository.Build(),
            unitOfWork: unitOfWork,
            mapper: mapper,
            passwordEncrypter: encrypter,
            tokenGenerator: accessTokenGenerator,
            emailService: null);
    }
}