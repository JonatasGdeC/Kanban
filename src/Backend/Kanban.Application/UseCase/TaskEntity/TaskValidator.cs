using FluentValidation;
using Kanban.Communication.Requests.Task;
using Kanban.Exception;

namespace Kanban.Application.UseCase.TaskEntity;

public class TaskValidator : AbstractValidator<RegisterTaskRequest>
{
    public TaskValidator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.NAME_IS_REQUIRED)
            .MinimumLength(minimumLength: 3).WithMessage(errorMessage: ResourceErrorMessage.NAME_MINIMUM_LENGTH)
            .MaximumLength(maximumLength: 200).WithMessage(errorMessage: ResourceErrorMessage.NAME_MAXIMUM_LENGTH);
    }
}
