using CommomTestsUtilies.Requests;
using CommomTestsUtilies.Utils.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Application.UseCase.Column;
using Kanban.Communication.Requests.Column;
using Kanban.Exception;

namespace Validators.Tests.Column;

public class ColumnValidatorTests
{
    [Fact]
    public void Success()
    {
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        ValidationResult? validator = new ColumnValidator().Validate(instance: request);

        validator.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_Name_Is_Empty(string name)
    {
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        request.Name = name;
        ValidationResult? validator = new ColumnValidator().Validate(instance: request);
        
        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, name: name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(201)]
    public void Error_Name_Length_Is_Invalid(int length)
    {
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        request.Name = new string(c: 'a', count: length);
        ValidationResult? validator = new ColumnValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, length: length);
    }
    
    
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_Color_Is_Empty(string color)
    {
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        request.Color = color;
        ValidationResult? validator = new ColumnValidator().Validate(instance: request);
        
        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And
            .Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.COLOR_IS_REQUIRED));
    }
    
    [Theory]
    [InlineData("000000")]
    [InlineData("#0000000")]
    [InlineData("#00%000")]
    public void Error_Color_Is_Invalid(string color)
    {
        RegisterColumnRequest request = RegisterColumnRequestBuilder.Build();
        request.Color = color;
        ValidationResult? validator = new ColumnValidator().Validate(instance: request);
        
        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And
            .Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.COLOR_INVALID_HEX));
    }
}