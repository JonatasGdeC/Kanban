using CommomTestsUtilies.Requests;
using CommomTestsUtilies.Utils.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Application.UseCase.SubTask;
using Kanban.Communication.Requests.SubTask;
using Kanban.Exception;

namespace Validators.Tests.SubTask;

public class SubTaskValidatorTests
{
    [Fact]
    public void Success()
    {
        RegisterSubTaskRequest request = RegisterSubTaskRequestBuilder.Build();
        ValidationResult? validator = new SubTaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_NameIsEmpty(string name)
    {
        RegisterSubTaskRequest request = RegisterSubTaskRequestBuilder.Build();
        request.Name = name;
        ValidationResult? validator = new SubTaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, name: name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(201)]
    public void Error_NameLengthIsInvalid(int length)
    {
        RegisterSubTaskRequest request = RegisterSubTaskRequestBuilder.Build();
        request.Name = new string(c: 'a', count: length);
        ValidationResult? validator = new SubTaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, length: length);
    }
}
