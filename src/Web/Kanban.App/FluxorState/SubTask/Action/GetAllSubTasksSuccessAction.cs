using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Action;

public record GetAllSubTasksSuccessAction(Guid TaskId, List<SubTaskDto> SubTasks);
