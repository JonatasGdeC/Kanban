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

namespace WebApi.Tests.User.Login;

using Kanban.Domain.Entities;

public class LoginTest(CustomWebApplicationFactory factory) : KanbanClassFixture(webApplicationFactory: factory)
{
    private const string Method = "User/login";

    [Fact]
    public async Task Success()
    {
        LoginRequest request = new()
        {
            Email = factory.User.Email,
            Password = factory.User.Password
        };

        HttpResponseMessage response = await DoPost(requestUri: Method, request: request);

        response.StatusCode.Should().Be(expected: HttpStatusCode.OK);

        Stream responseBody = await response.Content.ReadAsStreamAsync();
        JsonDocument responseData = await JsonDocument.ParseAsync(utf8Json: responseBody);

        responseData.RootElement.GetProperty(propertyName: nameof(LoginResponse.User).HandlePropertyName())
            .GetProperty(propertyName: nameof(LoginResponse.User.Name).HandlePropertyName()).GetString().Should()
            .Be(expected: factory.User.Name);

        responseData.RootElement.GetProperty(propertyName: nameof(LoginResponse.Token).HandlePropertyName()).GetString()
            .Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(@class: typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        User user = UserBuilder.Build();

        LoginRequest request = new()
        {
            Email = user.Email,
            Password = user.Password
        };

        HttpResponseMessage response = await DoPost(requestUri: Method, request: request, culture: culture);

        response.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);

        Stream responseBody = await response.Content.ReadAsStreamAsync();
        JsonDocument responseData = await JsonDocument.ParseAsync(utf8Json: responseBody);
        JsonElement.ArrayEnumerator errors = responseData.RootElement
            .GetProperty(propertyName: nameof(ErrorResponse.ErrorMessages).HandlePropertyName()).EnumerateArray();
        string? expectedMessage = ResourceErrorMessage.ResourceManager.GetString(
            name: nameof(ResourceErrorMessage.INVALID_LOGIN), culture: new CultureInfo(name: culture));

        errors.Should().HaveCount(expected: 1).And.Contain(predicate: c => c.GetString()!.Equals(expectedMessage));
    }
}