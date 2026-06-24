using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerRegisterTask
{
    [ReducerMethod]
    public static TaskListState ReduceRegisterTaskSuccess(TaskListState state, RegisterTaskSuccessAction action)
        => new() { IsLoading = false, Tasks = [..state.Tasks, action.Task] };
}
