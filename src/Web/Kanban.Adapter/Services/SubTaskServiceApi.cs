using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Responses.SubTask;

namespace Kanban.Adapter.Services;

public class SubTaskServiceApi(HttpClient httpClient) : ApiServiceBase(httpClient: httpClient)
{
    private const string BaseUri = "SubTask";

    public async Task<SubTaskDto> Register(Guid taskId, RegisterSubTaskRequest request)
    {
        return await PostAsync<RegisterSubTaskRequest, SubTaskDto>(uri: $"{BaseUri}/{taskId}", request: request);
    }

    public async Task<GetAllSubTasksResponse?> GetAll(Guid taskId)
    {
        return await GetAsync<GetAllSubTasksResponse>(uri: $"{BaseUri}/{taskId}");
    }

    public async Task Delete(Guid id)
    {
        await DeleteAsync(uri: $"{BaseUri}/{id}");
    }

    public async Task Update(Guid id, UpdateSubTaskRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/{id}", request: request);
    }
}
