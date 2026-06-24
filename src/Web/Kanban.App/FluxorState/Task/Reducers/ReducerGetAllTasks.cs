using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerGetAllTasks
{
    [ReducerMethod(actionType: typeof(GetAllTasksAction))]
    public static TaskListState ReduceGetAllTasks(TaskListState state)
        => new() { IsLoading = true, Tasks = state.Tasks };

    [ReducerMethod]
    public static TaskListState ReduceGetAllTasksSuccess(TaskListState state, GetAllTasksSuccessAction action)
        => new() { IsLoading = false, Tasks = action.Tasks };
}
