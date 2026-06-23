using Fluxor;
using Kanban.Adapter.Services;
using Kanban.App.FluxorState.Action.Board;
using Kanban.Communication.Responses.Board;

namespace Kanban.App.FluxorState.State.Board;

public class BoardEffects(BoardServiceApi api)
{
    [EffectMethod]
    public async Task LoadBoard(LoadBoardAction action, IDispatcher dispatcher)
    {
        GetBoardByIdResponse? response = await api.GetById(id: action.BoardId);

        if (response?.Board != null)
        {
            dispatcher.Dispatch(action: new LoadBoardSuccessAction(Board: response.Board));
        }
    }
}