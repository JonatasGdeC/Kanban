using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerRegisterSubTask
{
    [ReducerMethod]
    public static SubTaskListState ReduceRegisterSubTaskSuccess(SubTaskListState state, RegisterSubTaskSuccessAction action)
    {
        List<SubTaskDto> existing = state.SubTasksByTaskId.GetValueOrDefault(key: action.TaskId, defaultValue: []);
        Dictionary<Guid, List<SubTaskDto>> updated = new(dictionary: state.SubTasksByTaskId)
        {
            [key: action.TaskId] = [..existing, action.SubTask]
        };
        return new() { IsLoading = false, SubTasksByTaskId = updated };
    }
}
