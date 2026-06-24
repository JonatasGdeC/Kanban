using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Action;

public record RegisterSubTaskSuccessAction(Guid TaskId, SubTaskDto SubTask);
