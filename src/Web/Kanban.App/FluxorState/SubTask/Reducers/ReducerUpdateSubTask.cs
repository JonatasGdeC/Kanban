using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerUpdateSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceUpdateSubTaskSuccess(SubTaskListState state, UpdateSubTaskSuccessAction action)
        => new()
        {
            IsLoading = false,
            SubTasks = state.SubTasks.Select(selector: s => s.Id == action.SubTask.Id ? action.SubTask : s).ToList()
        };
}
