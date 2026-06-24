using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerDeleteSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceDeleteSubTaskSuccess(SubTaskListState state, DeleteSubTaskSuccessAction action)
    {
        Dictionary<Guid, List<SubTaskDto>> updated = state.SubTasksByTaskId.ToDictionary(
            keySelector: kvp => kvp.Key,
            elementSelector: kvp => kvp.Value.Where(predicate: s => s.Id != action.SubTaskId).ToList()
        );
        return new() { IsLoading = false, SubTasksByTaskId = updated };
    }
}
