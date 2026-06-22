using AutoMapper;
using FluentValidation.Results;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Security.Tokens;
using Kanban.Domain.Services.MailKit;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.User.Register;
using Domain.Entities;

public class RegisterUserUseCase(
    IUserWriteRepository writeRepository,
    IUserReadRepository readRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator,
    IEmailService emailService) : IRegisterUserUseCase
{
    public async Task<RegisteredUserResponse> Execute(RegisterUserRequest request)
    {
        await Validate(request: request);

        User user = mapper.Map<User>(source: request);
        user.Password = passwordEncrypter.Encrypt(value: request.Password);

        await writeRepository.Add(user: user);
        await unitOfWork.Commit();

        await emailService.SendWelcomeEmail(to: user.Email, userName: user.Name);
        
        return new RegisteredUserResponse
        {
            User = mapper.Map<UserDto>(source: user),
            Token = tokenGenerator.Generate(user: user)
        };
    }

    private async Task Validate(RegisterUserRequest request)
    {
        UserValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        User? existingUser = await readRepository.GetByEmail(email: request.Email);
        if (existingUser != null)
        {
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceErrorMessage.EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }
    }
}
