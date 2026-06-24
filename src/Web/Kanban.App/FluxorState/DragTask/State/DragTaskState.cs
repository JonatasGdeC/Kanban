using Fluxor;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.DragTask.State;

[FeatureState]
public record DragTaskState
{
    public TaskDto? DraggingTask { get; init; }
    public Guid SourceColumnId { get; init; }
    public bool IsDragging => DraggingTask != null;
}
