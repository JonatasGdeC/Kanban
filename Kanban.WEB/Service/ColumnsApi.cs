using System.Net.Http.Json;
using Kanban.WEB.Response;

namespace Kanban.WEB.Service;

public class ColumnsApi
{
  private readonly HttpClient _httpClient;

  public ColumnsApi(IHttpClientFactory factory)
  {
    _httpClient = factory.CreateClient("API");
  }

  public async Task<ICollection<ColumnsResponse>?> GetColumnByBoardIdAsync(int idBoard)
  {
    return await
      _httpClient.GetFromJsonAsync<ICollection<ColumnsResponse>>($"Columns/{idBoard}");
  }
}
