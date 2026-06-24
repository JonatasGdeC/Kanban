using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerUpdateTask
{
    [ReducerMethod]
    public static TaskListState ReduceUpdateTaskSuccess(TaskListState state, UpdateTaskSuccessAction action)
        => new()
        {
            IsLoading = false,
            Tasks = state.Tasks.Select(selector: t => t.Id == action.Task.Id ? action.Task : t).ToList()
        };

    [ReducerMethod]
    public static TaskState ReduceUpdateCurrentTaskSuccess(TaskState state, UpdateTaskSuccessAction action)
        => state.Task?.Id == action.Task.Id
            ? new() { IsLoading = false, Task = action.Task }
            : state;
}
