using Fluxor;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.FluxorState.Task.State;
using Kanban.Communication.Dtos;

namespace Kanban.App.FluxorState.Task.Reducers;

public static class ReducerGetAllTasks
{
    [ReducerMethod]
    public static TaskListState ReduceGetAllTasks(TaskListState state, GetAllTasksAction action)
        => new() { IsLoading = true, Tasks = state.Tasks.Where(predicate: t => t.ColumnId != action.ColumnId).ToList() };

    [ReducerMethod]
    public static TaskListState ReduceGetAllTasksSuccess(TaskListState state, GetAllTasksSuccessAction action)
    {
        Guid? columnId = action.Tasks.FirstOrDefault()?.ColumnId;
        List<TaskDto> remaining = columnId.HasValue
            ? state.Tasks.Where(predicate: t => t.ColumnId != columnId).ToList()
            : state.Tasks;
        return new() { IsLoading = false, Tasks = [..remaining, ..action.Tasks] };
    }
}
