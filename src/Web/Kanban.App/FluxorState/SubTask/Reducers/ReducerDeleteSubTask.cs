using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerDeleteSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceDeleteSubTaskSuccess(SubTaskListState state, DeleteSubTaskSuccessAction action)
        => new() { IsLoading = false, SubTasks = state.SubTasks.Where(predicate: s => s.Id != action.SubTaskId).ToList() };
}
