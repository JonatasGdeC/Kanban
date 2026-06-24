using Fluxor;
using Kanban.App.FluxorState.DragTask.Action;
using Kanban.App.FluxorState.DragTask.State;

namespace Kanban.App.FluxorState.DragTask.Reducers;

public static class ReducerDragTask
{
    [ReducerMethod]
    public static DragTaskState ReduceStartDragTask(DragTaskState state, StartDragTaskAction action)
        => new() { DraggingTask = action.Task, SourceColumnId = action.SourceColumnId };

    [ReducerMethod(actionType: typeof(ClearDragTaskAction))]
    public static DragTaskState ReduceClearDragTask(DragTaskState state)
        => new();
}
