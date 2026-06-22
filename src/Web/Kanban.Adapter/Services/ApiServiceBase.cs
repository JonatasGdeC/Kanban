using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Kanban.Adapter.Exceptions;
using Kanban.Communication.Responses;

namespace Kanban.Adapter.Services;

public abstract class ApiServiceBase(HttpClient httpClient)
{
  protected async Task<TResponse?> GetAsync<TResponse>(string uri)
  {
    AddLanguageHeader();
    using HttpResponseMessage response = await httpClient.GetAsync(requestUri: uri);
    return await ReadResponse<TResponse>(response: response);
  }
  
  protected async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request)
  {
    AddLanguageHeader();
    using HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri: uri, value: request);
    return await ReadResponse<TResponse>(response: response);
  }

  protected async Task PutAsync<TRequest>(string uri, TRequest request)
  {
    AddLanguageHeader();
    using HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUri: uri, value: request);
    await EnsureSuccessResponse(response: response);
  }

  protected async Task DeleteAsync(string uri)
  {
    AddLanguageHeader();
    using HttpResponseMessage response = await httpClient.DeleteAsync(requestUri: uri);
    await EnsureSuccessResponse(response: response);
  }

  private static async Task<TResponse> ReadResponse<TResponse>(HttpResponseMessage response)
  {
    await EnsureSuccessResponse(response: response);

    if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
        response.Content.Headers.ContentLength == 0)
    {
      return default!;
    }

    TResponse? content = await response.Content.ReadFromJsonAsync<TResponse>();

    if (content == null)
    {
      throw new InvalidOperationException(message: $"The API response for '{typeof(TResponse).Name}' was empty.");
    }

    return content;
  }

  private static async Task EnsureSuccessResponse(HttpResponseMessage response)
  {
    if (response.IsSuccessStatusCode)
    {
      return;
    }

    IReadOnlyList<string> errorMessages = await ReadErrorMessages(response: response);

    if (errorMessages.Count == 0)
    {
      errorMessages = [$"Request failed with status code {(int)response.StatusCode}."];
    }

    throw new ApiException(statusCode: response.StatusCode, errorMessages: errorMessages);
  }

  private static async Task<IReadOnlyList<string>> ReadErrorMessages(HttpResponseMessage response)
  {
    try
    {
      ErrorResponse? error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
      return error?.ErrorMessages.Where(predicate: message => !string.IsNullOrWhiteSpace(value: message)).ToList() ?? [];
    }
    catch (JsonException)
    {
      string rawContent = await response.Content.ReadAsStringAsync();
      return string.IsNullOrWhiteSpace(value: rawContent) ? [] : [rawContent];
    }
  }
  
  private void AddLanguageHeader()
  {
    string culture = CultureInfo.CurrentUICulture.Name;

    httpClient.DefaultRequestHeaders.Remove(name: "Accept-Language");
    httpClient.DefaultRequestHeaders.Add(name: "Accept-Language", value: culture);
  }
}