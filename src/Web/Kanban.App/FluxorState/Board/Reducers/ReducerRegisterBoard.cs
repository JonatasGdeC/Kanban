using Fluxor;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Board.State;

namespace Kanban.App.FluxorState.Board.Reducers;

public class ReducerRegisterBoard
{
    [ReducerMethod]
    public static BoardListState ReduceRegisterBoardSuccess(BoardListState state, RegisterBoardSuccessAction action)
        => new() { IsLoading = false, Boards = [..state.Boards, action.Board] };
}