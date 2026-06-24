using Fluxor;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.FluxorState.Column.State;

namespace Kanban.App.FluxorState.Column.Reducers;

public class ReducerUpdateColumn
{
    [ReducerMethod]
    public static ColumnListState ReduceUpdateColumnSuccess(ColumnListState state, UpdateColumnSuccessAction action)
        => new()
        {
            IsLoading = false,
            Columns = state.Columns.Select(selector: c => c.Id == action.Column.Id ? action.Column : c).ToList()
        };

    [ReducerMethod]
    public static ColumnState ReduceUpdateCurrentColumnSuccess(ColumnState state, UpdateColumnSuccessAction action)
        => state.Column?.Id == action.Column.Id
            ? new() { IsLoading = false, Column = action.Column }
            : state;
}