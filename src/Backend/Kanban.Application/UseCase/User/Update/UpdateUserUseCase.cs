using FluentValidation.Results;
using Kanban.Communication.Requests.User;
using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.User;
using Kanban.Domain.Security.Cryptography;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.User.Update;
using Domain.Entities;

public class UpdateUserUseCase(
    IUserWriteRepository writeRepository,
    IUserReadRepository readRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    public async Task Execute(UpdateUserRequest request)
    {
        User user = await loggedUser.Get();
        await Validate(request: request, currentUserId: user.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        writeRepository.Update(user: user);
        await unitOfWork.Commit();
    }

    private async Task Validate(UpdateUserRequest request, Guid currentUserId)
    {
        UpdateUserValidator validator = new();
        ValidationResult? result = await validator.ValidateAsync(instance: request);

        User? existingUser = await readRepository.GetByEmail(email: request.Email);
        if (existingUser != null && existingUser.Id != currentUserId)
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
