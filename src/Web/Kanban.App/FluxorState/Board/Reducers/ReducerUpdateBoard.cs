using Fluxor;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Board.State;

namespace Kanban.App.FluxorState.Board.Reducers;

public class ReducerUpdateBoard
{
    [ReducerMethod]
    public static BoardListState ReduceUpdateBoardSuccess(BoardListState state, UpdateBoardSuccessAction action)
        => new()
        {
            IsLoading = false,
            Boards = state.Boards.Select(selector: b => b.Id == action.Board.Id ? action.Board : b).ToList()
        };

    [ReducerMethod]
    public static BoardState ReduceUpdateCurrentBoardSuccess(BoardState state, UpdateBoardSuccessAction action)
        => state.Board?.Id == action.Board.Id
            ? new() { IsLoading = false, Board = action.Board }
            : state;
}