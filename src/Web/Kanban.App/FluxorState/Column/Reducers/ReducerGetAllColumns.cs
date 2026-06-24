using Fluxor;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.FluxorState.Column.State;

namespace Kanban.App.FluxorState.Column.Reducers;

public class ReducerGetAllColumns
{
    [ReducerMethod(actionType: typeof(GetAllColumnsAction))]
    public static ColumnListState ReduceGetAllColumns(ColumnListState state)
        => new() { IsLoading = true, Columns = [] };

    [ReducerMethod]
    public static ColumnListState ReduceGetAllColumnsSuccess(ColumnListState state, GetAllColumnsSuccessAction action)
        => new() { IsLoading = false, Columns = action.Columns };
}