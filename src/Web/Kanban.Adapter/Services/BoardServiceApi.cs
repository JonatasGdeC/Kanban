using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Responses.Board;

namespace Kanban.Adapter.Services;

public class BoardServiceApi(HttpClient httpClient) : ApiServiceBase(httpClient: httpClient)
{
    private const string BaseUri = "Board";
    
    public async Task<BoardDto> Register(RegisterBoardRequest board)
    {
        return await PostAsync<RegisterBoardRequest, BoardDto>(uri: BaseUri, request: board);
    }
    
    public async Task<GetAllBoardsResponse?> GetAll()
    {
        return await GetAsync<GetAllBoardsResponse>(uri: BaseUri);
    }
    
    public async Task<GetBoardByIdResponse?> GetById(Guid id)
    {
        return await GetAsync<GetBoardByIdResponse>(uri: $"{BaseUri}/{id}");
    }
    
    public async Task Delete(Guid id)
    {
        await DeleteAsync(uri: $"{BaseUri}/{id}");
    }

    public async Task Update(Guid id, RegisterBoardRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/{id}", request: request);
    }
}