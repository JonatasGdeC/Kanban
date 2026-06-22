using FluentValidation;
using Kanban.Communication.Requests.Board;
using Kanban.Exception;

namespace Kanban.Application.UseCase.Board;

public class BoardValidator : AbstractValidator<RegisterBoardRequest>
{
    public BoardValidator()
    {
        RuleFor(expression: request => request.Name)
            .NotEmpty().WithMessage(errorMessage: ResourceErrorMessage.NAME_IS_REQUIRED)
            .MinimumLength(minimumLength: 3).WithMessage(errorMessage: ResourceErrorMessage.NAME_MINIMUM_LENGTH)
            .MaximumLength(maximumLength: 200).WithMessage(errorMessage: ResourceErrorMessage.NAME_MAXIMUM_LENGTH);
    }
}