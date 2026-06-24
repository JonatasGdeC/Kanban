using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerUpdateSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceUpdateSubTaskSuccess(SubTaskListState state, UpdateSubTaskSuccessAction action)
    {
        Dictionary<Guid, List<SubTaskDto>> updated = state.SubTasksByTaskId.ToDictionary(
            keySelector: kvp => kvp.Key,
            elementSelector: kvp => kvp.Value
                .Select(selector: s => s.Id == action.SubTask.Id ? action.SubTask : s)
                .ToList()
        );
        return new() { IsLoading = false, SubTasksByTaskId = updated };
    }
}
