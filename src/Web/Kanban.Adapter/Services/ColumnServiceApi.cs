using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Communication.Responses.Column;

namespace Kanban.Adapter.Services;

public class ColumnServiceApi(HttpClient httpClient) : ApiServiceBase(httpClient: httpClient)
{
    private const string BaseUri = "Column";

    public async Task<ColumnDto> Register(Guid boardId, RegisterColumnRequest request)
    {
        return await PostAsync<RegisterColumnRequest, ColumnDto>(uri: $"{BaseUri}/{boardId}", request: request);
    }

    public async Task<GetAllColumnsResponse?> GetAll(Guid boardId)
    {
        return await GetAsync<GetAllColumnsResponse>(uri: $"{BaseUri}/{boardId}");
    }

    public async Task<GetColumnByIdResponse?> GetById(Guid id)
    {
        return await GetAsync<GetColumnByIdResponse>(uri: $"{BaseUri}/details/{id}");
    }

    public async Task Delete(Guid id)
    {
        await DeleteAsync(uri: $"{BaseUri}/{id}");
    }

    public async Task Update(Guid id, UpdateColumnRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/{id}", request: request);
    }
}
