using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerDeleteTask
{
    [ReducerMethod]
    public static TaskListState ReduceDeleteTaskSuccess(TaskListState state, DeleteTaskSuccessAction action)
        => new() { IsLoading = false, Tasks = state.Tasks.Where(predicate: t => t.Id != action.TaskId).ToList() };

    [ReducerMethod]
    public static TaskState ReduceDeleteCurrentTaskSuccess(TaskState state, DeleteTaskSuccessAction action)
        => state.Task?.Id == action.TaskId
            ? new() { IsLoading = false, Task = null }
            : state;
}
