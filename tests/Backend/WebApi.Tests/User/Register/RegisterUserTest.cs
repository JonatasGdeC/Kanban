using System.Globalization;
using System.Net;
using System.Text.Json;
using CommomTestsUtilies.Entities;
using FluentAssertions;
using Kanban.Communication.Requests.User;
using Kanban.Communication.Responses;
using Kanban.Communication.Responses.User;
using Kanban.Exception;
using WebApi.Tests.InlineData;
using WebApi.Tests.Utils;

namespace WebApi.Tests.User.Register;
using Kanban.Domain.Entities;

public class RegisterUserTest(CustomWebApplicationFactory factory) : KanbanClassFixture(webApplicationFactory: factory)
{
    private const string Method = "User";
    
    [Fact]
    public async Task Success()
    {
        User newUser = UserBuilder.Build();
        
        RegisterUserRequest request = new()
        {
            Name = newUser.Name,
            Email = newUser.Email,
            Password = newUser.Password
        };

        HttpResponseMessage response = await DoPost(requestUri: Method, request: request);

        response.StatusCode.Should().Be(expected: HttpStatusCode.Created);

        Stream responseBody = await response.Content.ReadAsStreamAsync();
        JsonDocument responseData = await JsonDocument.ParseAsync(utf8Json: responseBody);

        responseData.RootElement.GetProperty(propertyName: nameof(RegisteredUserResponse.User).HandlePropertyName())
            .GetProperty(propertyName: nameof(RegisteredUserResponse.User.Name).HandlePropertyName()).GetString().Should()
            .Be(expected: newUser.Name);
        
        responseData.RootElement.GetProperty(propertyName: nameof(RegisteredUserResponse.User).HandlePropertyName())
            .GetProperty(propertyName: nameof(RegisteredUserResponse.User.Email).HandlePropertyName()).GetString().Should()
            .Be(expected: newUser.Email);

        responseData.RootElement.GetProperty(propertyName: nameof(RegisteredUserResponse.Token).HandlePropertyName()).GetString()
            .Should().NotBeNullOrEmpty();
    }
    
    
    [Theory]
    [ClassData(@class: typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Exists(string culture)
    {
        
        RegisterUserRequest request = new()
        {
            Name = factory.User.Name,
            Email = factory.User.Email,
            Password = factory.User.Password
        };

        HttpResponseMessage response = await DoPost(requestUri: Method, request: request, culture: culture);

        response.StatusCode.Should().Be(expected: HttpStatusCode.BadRequest);
        
        Stream responseBody = await response.Content.ReadAsStreamAsync();
        JsonDocument responseData = await JsonDocument.ParseAsync(utf8Json: responseBody);
        JsonElement.ArrayEnumerator errors = responseData.RootElement
            .GetProperty(propertyName: nameof(ErrorResponse.ErrorMessages).HandlePropertyName()).EnumerateArray();
        string? expectedMessage = ResourceErrorMessage.ResourceManager.GetString(
            name: nameof(ResourceErrorMessage.EMAIL_ALREADY_EXISTS), culture: new CultureInfo(name: culture));

        errors.Should().HaveCount(expected: 1).And.Contain(predicate: c => c.GetString()!.Equals(expectedMessage));
    }
}