using Fluxor;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.FluxorState.Column.State;

namespace Kanban.App.FluxorState.Column.Reducers;

public class ReducerRegisterColumn
{
    [ReducerMethod]
    public static ColumnListState ReduceRegisterColumnSuccess(ColumnListState state, RegisterColumnSuccessAction action)
        => new() { IsLoading = false, Columns = [..state.Columns, action.Column] };
}