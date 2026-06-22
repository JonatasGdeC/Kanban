using FluentValidation;
using Kanban.Communication.Requests.User;
using Kanban.Exception;

namespace Kanban.Application.UseCase.User.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.NAME_IS_REQUIRED)
            .MinimumLength(minimumLength: 3).WithMessage(errorMessage: ResourceErrorMessage.NAME_MINIMUM_LENGTH)
            .MaximumLength(maximumLength: 200).WithMessage(errorMessage: ResourceErrorMessage.NAME_MAXIMUM_LENGTH);

        RuleFor(expression: request => request.Email)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.EMAIL_IS_REQUIRED)
            .EmailAddress().WithMessage(errorMessage: ResourceErrorMessage.EMAIL_INVALID);
    }
}
