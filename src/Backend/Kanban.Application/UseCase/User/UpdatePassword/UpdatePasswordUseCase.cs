using FluentValidation.Results;
using Kanban.Communication.Requests.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.User.UpdatePassword;
using Domain.Entities;

public class UpdatePasswordUseCase(
    ILoggedUser loggedUser,
    IEncrypter encrypter,
    IUserWriteRepository writeRepository,
    IUnitOfWork unitOfWork) : IUpdatePasswordUseCase
{
    public async Task Execute(UpdatePasswordRequest request)
    {
        User user = await loggedUser.Get();
        
        Validate(request: request, user: user);
        
        user.Password = encrypter.Encrypt(value: request.NewPassword);
        writeRepository.Update(user: user);
        await unitOfWork.Commit();
    }
    
    private void Validate(UpdatePasswordRequest request, User user)
    {
        ValidationResult resultPassword = new PasswordValidator().Validate(instance: request.NewPassword);

        if (!resultPassword.IsValid)
        {
            List<string> errors = resultPassword.Errors.Select(selector: error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages: errors);
        }
        
        bool passwordMatch = encrypter.Verify(value: request.OldPassword, hash: user.Password);
        if (!passwordMatch)
        {
            throw new BadRequestException(message: ResourceErrorMessage.OLD_PASSWORD_INVALID);
        }
    }
}