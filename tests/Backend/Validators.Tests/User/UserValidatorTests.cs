using CommomTestsUtilies.Requests;
using CommomTestsUtilies.Utils.Validators;
using FluentAssertions;
using FluentValidation.Results;
using Kanban.Application.UseCase.User;
using Kanban.Communication.Requests.User;
using Kanban.Exception;

namespace Validators.Tests.User;

public class UserValidatorTests
{
    [Fact]
    public void Success()
    {
        ValidationResult? validator = new UserValidator().Validate(instance: RegisterUserRequestBuilder.Build());
        validator.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_NameIsRequired(string name)
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        request.Name = name;

        ValidationResult? validator = new UserValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, name: name);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(201)]
    public void Error_NameLengthInvalid(int length)
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        request.Name = new string(c: 'a', count: length);

        ValidationResult? validator = new UserValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        ErrorNameMessage.Execute(errors: validator.Errors, length: length);
    }
    
    
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(data: null)]
    public void Error_EmailIsRequired(string email)
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        request.Email = email;

        ValidationResult? validator = new UserValidator().Validate(instance: request);

        validator.IsValid.Should().BeFalse();
        validator.Errors.Should().ContainSingle().And
            .Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.EMAIL_IS_REQUIRED));
    }
    
    
    [Fact]
    public void Error_Email_Invalid()
    {
        RegisterUserRequest request = RegisterUserRequestBuilder.Build();
        string newEmail = request.Email.Replace(oldChar: '@', newChar: '.');
        request.Email = newEmail;

        ValidationResult? result = new UserValidator().Validate(instance: request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(predicate: e => e.ErrorMessage.Equals(ResourceErrorMessage.EMAIL_INVALID));
    }
}