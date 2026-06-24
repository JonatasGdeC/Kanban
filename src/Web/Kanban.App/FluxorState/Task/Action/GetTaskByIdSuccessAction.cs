using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Task.Action;

public record GetTaskByIdSuccessAction(TaskDto Task);
