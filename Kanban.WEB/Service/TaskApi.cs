using System.Net.Http.Json;
using Kanban.WEB.Response;

namespace Kanban.WEB.Service;

public class TaskApi
{
  private readonly HttpClient _httpClient;

  public TaskApi(IHttpClientFactory factory)
  {
    _httpClient = factory.CreateClient("API");
  }

  public async Task<ICollection<TaskResponse>?> GetTaskByColumnIdAsync(int idColumn)
  {
    return await
      _httpClient.GetFromJsonAsync<ICollection<TaskResponse>>($"tasks/{idColumn}");
  }
}
