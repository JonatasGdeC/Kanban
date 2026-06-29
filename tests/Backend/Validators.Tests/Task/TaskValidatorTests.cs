using CommomTestsUtilies.Requests;
using CommomTestsUtilies.Utils.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Application.UseCase.TaskEntity;
using Kanban.Communication.Requests.Task;
using Kanban.Exception;

namespace Validators.Tests.Task;

public class TaskValidatorTests
{
    [Fact]
    public void Success()
    {
        RegisterTaskRequest request = RegisterTaskRequestBuilder.Build();
        ValidationResult? validator = new TaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_Name_Is_Empty(string name)
    {
        RegisterTaskRequest request = RegisterTaskRequestBuilder.Build();
        request.Name = name;
        ValidationResult? validator = new TaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, name: name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(201)]
    public void Error_Name_Length_Is_Invalid(int length)
    {
        RegisterTaskRequest request = RegisterTaskRequestBuilder.Build();
        request.Name = new string(c: 'a', count: length);
        ValidationResult? validator = new TaskValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, length: length);
    }
}
