using Fluxor;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Board.State;

namespace Kanban.App.FluxorState.Board.Reducers;

public class ReducerDeleteBoard
{
    [ReducerMethod]
    public static BoardListState ReduceDeleteBoardSuccess(BoardListState state, DeleteBoardSuccessAction action)
        => new() { IsLoading = false, Boards = state.Boards.Where(predicate: b => b.Id != action.BoardId).ToList() };

    [ReducerMethod]
    public static BoardState ReduceDeleteCurrentBoardSuccess(BoardState state, DeleteBoardSuccessAction action)
        => state.Board?.Id == action.BoardId
            ? new() { IsLoading = false, Board = null }
            : state;
}