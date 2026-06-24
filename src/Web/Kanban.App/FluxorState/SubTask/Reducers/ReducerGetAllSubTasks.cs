using Fluxor;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.SubTask.State;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.SubTask.Reducers;

public static class ReducerGetAllSubTasks
{
    [ReducerMethod]
    public static SubTaskListState ReduceGetAllSubTasks(SubTaskListState state, GetAllSubTasksAction action)
    {
        Dictionary<Guid, List<SubTaskDto>> updated = new(dictionary: state.SubTasksByTaskId)
        {
            [key: action.TaskId] = []
        };
        return new() { IsLoading = true, SubTasksByTaskId = updated };
    }

    [ReducerMethod]
    public static SubTaskListState ReduceGetAllSubTasksSuccess(SubTaskListState state, GetAllSubTasksSuccessAction action)
    {
        Dictionary<Guid, List<SubTaskDto>> updated = new(dictionary: state.SubTasksByTaskId)
        {
            [key: action.TaskId] = action.SubTasks
        };
        return new() { IsLoading = false, SubTasksByTaskId = updated };
    }
}
