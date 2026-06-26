using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Exception;

namespace CommomTestsUtilies.Utils.Validators;

public static class ErrorNameMessage
{
    public static void Execute(List<ValidationFailure> errors, int length)
    {
        if (length <= 2)
        {
            errors.Should().ContainSingle().And
                .Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.NAME_MINIMUM_LENGTH));
        }

        if (length >= 201)
        {
            errors.Should().ContainSingle().And
                .Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.NAME_MAXIMUM_LENGTH));
        }
    }
}