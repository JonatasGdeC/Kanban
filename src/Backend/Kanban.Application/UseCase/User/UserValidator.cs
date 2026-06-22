using FluentValidation;
using Kanban.Application.UseCase.User.Update;
using Kanban.Communication.Requests.User;

namespace Kanban.Application.UseCase.User;

public class UserValidator : AbstractValidator<RegisterUserRequest>
{
    public UserValidator()
    {
        RuleFor(expression: request => request).SetValidator(validator: new UpdateUserValidator());
        RuleFor(expression: request => request.Password).SetValidator(validator: new PasswordValidator());
    }
}
