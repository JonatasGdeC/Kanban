using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerRegisterSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceRegisterSubTaskSuccess(SubTaskListState state, RegisterSubTaskSuccessAction action)
        => new() { IsLoading = false, SubTasks = [..state.SubTasks, action.SubTask] };
}
