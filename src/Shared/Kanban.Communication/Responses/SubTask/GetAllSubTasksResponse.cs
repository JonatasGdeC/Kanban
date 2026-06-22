using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.SubTask;

public record GetAllSubTasksResponse
{
    public List<SubTaskDto> ListSubTasks { get; init; } = [];
}
