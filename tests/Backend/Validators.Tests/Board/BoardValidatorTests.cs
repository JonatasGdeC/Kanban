using CommomTestsUtilies.Requests;
using CommomTestsUtilies.Utils.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Application.UseCase.Board;
using Kanban.Communication.Requests.Board;

namespace Validators.Tests.Board;

public class BoardValidatorTests
{
    [Fact]
    public void Success()
    {
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        ValidationResult? validate = new BoardValidator().Validate(instance: request);

        validate.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_Name_Is_Empty(string name)
    {
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        request.Name = name;
        ValidationResult? validator = new BoardValidator().Validate(instance: request);
        
        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, name: name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(201)]
    public void Error_Name_Length_Is_Invalid(int length)
    {
        RegisterBoardRequest request = RegisterBoardRequestBuilder.Build();
        request.Name = new string(c: 'a', count: length);
        ValidationResult? validator = new BoardValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, length: length);
    }
}