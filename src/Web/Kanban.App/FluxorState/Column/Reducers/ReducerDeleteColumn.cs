using Fluxor;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.FluxorState.Column.State;

namespace Kanban.App.FluxorState.Column.Reducers;

public class ReducerDeleteColumn
{
    [ReducerMethod]
    public static ColumnListState ReduceDeleteColumnSuccess(ColumnListState state, DeleteColumnSuccessAction action)
        => new() { IsLoading = false, Columns = state.Columns.Where(predicate: c => c.Id != action.ColumnId).ToList() };

    [ReducerMethod]
    public static ColumnState ReduceDeleteCurrentColumnSuccess(ColumnState state, DeleteColumnSuccessAction action)
        => state.Column?.Id == action.ColumnId
            ? new() { IsLoading = false, Column = null }
            : state;
}