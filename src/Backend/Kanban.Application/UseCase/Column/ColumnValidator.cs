using FluentValidation;
using Kanban.Communication.Requests.Column;
using Kanban.Exception;

namespace Kanban.Application.UseCase.Column;

public class ColumnValidator : AbstractValidator<RegisterColumnRequest>
{
    public ColumnValidator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.NAME_IS_REQUIRED)
            .MinimumLength(minimumLength: 3).WithMessage(errorMessage: ResourceErrorMessage.NAME_MINIMUM_LENGTH)
            .MaximumLength(maximumLength: 200).WithMessage(errorMessage: ResourceErrorMessage.NAME_MAXIMUM_LENGTH);
        
        RuleFor(expression: request => request.Color)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.COLOR_IS_REQUIRED)
            .Matches(expression: @"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$")
            .WithMessage(errorMessage: ResourceErrorMessage.COLOR_INVALID_HEX);
    }
}