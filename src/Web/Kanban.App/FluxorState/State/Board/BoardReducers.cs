using Fluxor;
using Kanban.App.FluxorState.Action.Board.GetAll;
using Kanban.App.FluxorState.Action.Board.GetById;

namespace Kanban.App.FluxorState.State.Board;

public static class BoardReducers
{
    [ReducerMethod(actionType: typeof(GetBoardByIdAction))]
    public static BoardState ReduceLoadGetBoardById(BoardState state) 
        => new() { IsLoading = true, Board = null };

    [ReducerMethod]
    public static BoardState ReduceLoadGetBoardByIdSuccess(BoardState state, GetBoardByIdSuccessAction action) 
        => new() { IsLoading = false, Board = action.Board };

    [ReducerMethod(actionType: typeof(GetAllBoardsAction))]
    public static BoardListState ReduceLoadGetAllBoards(BoardListState state) 
        => new() { IsLoading = true, Boards = [] };
    
    [ReducerMethod]
    public static BoardListState ReduceLoadGetAllBoardsSuccess(BoardListState state, GetAllBoardsSuccessAction action)
        => new() { IsLoading = false, Boards = action.Boards };
}