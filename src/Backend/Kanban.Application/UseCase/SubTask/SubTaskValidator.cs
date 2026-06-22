using FluentValidation;
using Kanban.Communication.Requests.SubTask;
using Kanban.Exception;

namespace Kanban.Application.UseCase.SubTask;

public class SubTaskValidator : AbstractValidator<RegisterSubTaskRequest>
{
    public SubTaskValidator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.NAME_IS_REQUIRED)
            .MinimumLength(minimumLength: 3).WithMessage(errorMessage: ResourceErrorMessage.NAME_MINIMUM_LENGTH)
            .MaximumLength(maximumLength: 200).WithMessage(errorMessage: ResourceErrorMessage.NAME_MAXIMUM_LENGTH);
    }
}
