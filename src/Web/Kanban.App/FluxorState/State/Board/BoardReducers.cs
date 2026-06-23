using Fluxor;
using Kanban.App.FluxorState.Action.Board;

namespace Kanban.App.FluxorState.State.Board;

public static class BoardReducers
{
    [ReducerMethod(actionType: typeof(LoadBoardAction))]
    public static BoardState ReduceLoad(BoardState state) =>
        new() { IsLoading = true, Board = null };

    [ReducerMethod]
    public static BoardState ReduceLoadSuccess(BoardState state, LoadBoardSuccessAction action) =>
        new() { IsLoading = false, Board = action.Board };
}