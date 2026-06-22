using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.Task;

public class GetTaskByIdResponse
{
    public required TaskDto Task { get; init; }
    public List<SubTaskDto> SubTasks { get; init; } = [];
}
