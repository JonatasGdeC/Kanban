using System.Net.Http.Json;
using Kanban.WEB.Request;
using Kanban.WEB.Response;
using Newtonsoft.Json;

namespace Kanban.WEB.Service;

public class BoardApi
{
  private readonly HttpClient _httpClient;

  public BoardApi(IHttpClientFactory factory)
  {
    _httpClient = factory.CreateClient("API");
  }

  public async Task<ICollection<BoardsResponse>?> GetBoardsAsync()
  {
    return await
      _httpClient.GetFromJsonAsync<ICollection<BoardsResponse>>("boards");
  }

  public async Task<BoardsResponse> GetBoardsByIdAsync(int boardId)
  {
    var response = await _httpClient.GetAsync($"boards/{boardId}");
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<BoardsResponse>(responseBody);
  }

  public async Task<int> AddBoardAsync(BoardsRequest request)
  {
    var response = await _httpClient.PostAsJsonAsync("boards", request);
    response.EnsureSuccessStatusCode();
    await Task.Delay(1000);
    var boards = await GetBoardsAsync();
    var lastBoard = boards.OrderByDescending(b => b.Id).FirstOrDefault();

    return lastBoard?.Id ?? 0;
  }

  public async Task DeleteBoardAsync(int id)
  {
    await _httpClient.DeleteAsync($"boards/{id}");
  }
}
