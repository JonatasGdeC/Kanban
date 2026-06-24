using Fluxor;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Board.State;

namespace Kanban.App.FluxorState.Board.Reducers;

public class ReducerGetBoardById
{
    [ReducerMethod(actionType: typeof(GetBoardByIdAction))]
    public static BoardState ReduceGetBoardById(BoardState state)
        => new() { IsLoading = true, Board = null };

    [ReducerMethod]
    public static BoardState ReduceGetBoardByIdSuccess(BoardState state, GetBoardByIdSuccessAction action)
        => new() { IsLoading = false, Board = action.Board };
}