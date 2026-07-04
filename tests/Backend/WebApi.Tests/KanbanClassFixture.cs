using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Tests;

public class KanbanClassFixture(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizeRequest(token: token);
        ChangeRequestCulture(culture: culture);

        return await _httpClient.PostAsJsonAsync(requestUri: requestUri, value: request);
    }

    protected async Task<HttpResponseMessage> DoPut(string requestUri, object request, string token, string culture = "en")
    {
        AuthorizeRequest(token: token);
        ChangeRequestCulture(culture: culture);

        return await _httpClient.PutAsJsonAsync(requestUri: requestUri, value: request);
    }

    protected async Task<HttpResponseMessage> DoGet(string requestUri, string token, string culture = "en")
    {
        AuthorizeRequest(token: token);
        ChangeRequestCulture(culture: culture);

        return await _httpClient.GetAsync(requestUri: requestUri);
    }

    protected async Task<HttpResponseMessage> DoDelete(string requestUri, string token, string culture = "en")
    {
        AuthorizeRequest(token: token);
        ChangeRequestCulture(culture: culture);

        return await _httpClient.DeleteAsync(requestUri: requestUri);
    }

    private void AuthorizeRequest(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if(string.IsNullOrWhiteSpace(value: token))
        {
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
    }
    
    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(item: new StringWithQualityHeaderValue(value: culture));
    }
}
