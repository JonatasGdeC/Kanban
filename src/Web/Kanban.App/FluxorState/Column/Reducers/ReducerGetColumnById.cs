using Fluxor;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.FluxorState.Column.State;

namespace Kanban.App.FluxorState.Column.Reducers;

public class ReducerGetColumnById
{
    [ReducerMethod(actionType: typeof(GetColumnByIdAction))]
    public static ColumnState ReduceGetColumnById(ColumnState state)
        => new() { IsLoading = true, Column = null };

    [ReducerMethod]
    public static ColumnState ReduceGetColumnByIdSuccess(ColumnState state, GetColumnByIdSuccessAction action)
        => new() { IsLoading = false, Column = action.Column };
}