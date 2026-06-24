using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerGetAllSubTasks
{
    [ReducerMethod(actionType: typeof(GetAllSubTasksAction))]
    public static SubTaskListState ReduceGetAllSubTasks(SubTaskListState state)
        => new() { IsLoading = true, SubTasks = state.SubTasks };

    [ReducerMethod]
    public static SubTaskListState ReduceGetAllSubTasksSuccess(SubTaskListState state, GetAllSubTasksSuccessAction action)
        => new() { IsLoading = false, SubTasks = action.SubTasks };
}
