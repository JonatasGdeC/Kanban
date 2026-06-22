using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;
using Kanban.Communication.Responses.Task;

namespace Kanban.Adapter.Services;

public class TaskServiceApi(HttpClient httpClient) : ApiServiceBase(httpClient: httpClient)
{
    private const string BaseUri = "Task";

    public async Task<TaskDto> Register(Guid columnId, RegisterTaskRequest request)
    {
        return await PostAsync<RegisterTaskRequest, TaskDto>(uri: $"{BaseUri}/{columnId}", request: request);
    }

    public async Task<GetAllTasksResponse?> GetAll(Guid columnId)
    {
        return await GetAsync<GetAllTasksResponse>(uri: $"{BaseUri}/{columnId}");
    }

    public async Task<GetTaskByIdResponse?> GetById(Guid id)
    {
        return await GetAsync<GetTaskByIdResponse>(uri: $"{BaseUri}/details/{id}");
    }

    public async Task Delete(Guid id)
    {
        await DeleteAsync(uri: $"{BaseUri}/{id}");
    }

    public async Task Update(Guid id, UpdateTaskRequest request)
    {
        await PutAsync(uri: $"{BaseUri}/{id}", request: request);
    }
}
