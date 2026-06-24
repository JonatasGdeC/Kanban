using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Action;

public record GetAllSubTasksSuccessAction(List<SubTaskDto> SubTasks);
