using Fluxor;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Board.State;

namespace Kanban.App.FluxorState.Board.Reducers;

public class ReducerGetAllBoard
{
    [ReducerMethod(actionType: typeof(GetAllBoardsAction))]
    public static BoardListState ReduceGetAllBoards(BoardListState state)
        => new() { IsLoading = true, Boards = state.Boards };

    [ReducerMethod]
    public static BoardListState ReduceGetAllBoardsSuccess(BoardListState state, GetAllBoardsSuccessAction action)
        => new() { IsLoading = false, Boards = action.Boards };
}