using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Task;

public record GetAllTasksResponse
{
    public List<TaskDto> ListTasks { get; init; } = [];
}
