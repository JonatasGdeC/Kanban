using FluentValidation;
using Kanban.Exception;

namespace Kanban.Application.UseCase.User;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(expression: password => password)
            .NotEmpty()
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_IS_REQUIRED)
            .MinimumLength(minimumLength: 8)
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_MINIMUM_LENGTH)
            .Matches(expression: @"[a-z]")
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_MUST_CONTAIN_LOWERCASE_LETTER)
            .Matches(expression: @"[A-Z]")
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_MUST_CONTAIN_UPPERCASE_LETTER)
            .Matches(expression: @"[0-9]")
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_MUST_CONTAIN_NUMBER)
            .Matches(expression: @"[\W_]")
            .WithMessage(errorMessage: ResourceErrorMessage.PASSWORD_MUST_CONTAIN_SPECIAL_CHARACTER);
    }
}