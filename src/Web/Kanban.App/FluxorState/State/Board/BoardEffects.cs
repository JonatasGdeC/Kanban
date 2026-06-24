using Fluxor;
using Kanban.Adapter.Services;
using Kanban.App.FluxorState.Action.Board.GetAll;
using Kanban.App.FluxorState.Action.Board.GetById;
using Kanban.Communication.Responses.Board;

namespace Kanban.App.FluxorState.State.Board;

public class BoardEffects(BoardServiceApi api)
{
    [EffectMethod]
    public async Task GetAllBoards(GetAllBoardsAction action, IDispatcher dispatcher)
    {
        GetAllBoardsResponse? response = await api.GetAll();
        
        if (response != null)
        {
            dispatcher.Dispatch(action: new GetAllBoardsSuccessAction(Boards: response.ListBoards));
        }
    }
    
    [EffectMethod]
    public async Task GetBoardById(GetBoardByIdAction action, IDispatcher dispatcher)
    {
        GetBoardByIdResponse? response = await api.GetById(id: action.BoardId);

        if (response?.Board != null)
        {
            dispatcher.Dispatch(action: new GetBoardByIdSuccessAction(Board: response.Board));
        }
    }
}