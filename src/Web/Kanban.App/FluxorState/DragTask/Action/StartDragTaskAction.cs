using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.DragTask.Action;

public record StartDragTaskAction(TaskDto Task, Guid SourceColumnId);
