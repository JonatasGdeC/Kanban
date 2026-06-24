using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerGetTaskById
{
    [ReducerMethod(actionType: typeof(GetTaskByIdAction))]
    public static TaskState ReduceGetTaskById(TaskState state)
        => new() { IsLoading = true, Task = null };

    [ReducerMethod]
    public static TaskState ReduceGetTaskByIdSuccess(TaskState state, GetTaskByIdSuccessAction action)
        => new() { IsLoading = false, Task = action.Task };
}
