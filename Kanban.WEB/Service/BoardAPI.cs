using System.Net.Http.Json;
using Kanban.WEB.Request;
using Kanban.WEB.Response;

namespace Kanban.WEB.Service;

public class BoardAPI
{
  private readonly HttpClient _httpClient;

  public BoardAPI(IHttpClientFactory factory)
  {
    _httpClient = factory.CreateClient("API");
  }

  public async Task<ICollection<BoardsResponse>?> GetBoardsAsync()
  {
    return await
      _httpClient.GetFromJsonAsync<ICollection<BoardsResponse>>("boards");
  }

  public async Task AddBoardAsync(BoardsRequest boards)
  {
    await _httpClient.PostAsJsonAsync("boards", boards);
  }
}
