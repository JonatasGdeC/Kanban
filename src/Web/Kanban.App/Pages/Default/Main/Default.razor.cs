using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Communication.Responses.Board;
using Kanban.Communication.Responses.Column;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Pages.Default.Main;

public partial class Default : FluxorComponent
{
    [Parameter] public Guid? BoardId { get; set; }
    
    private Guid? _currentBoardId;

    protected override async Task OnParametersSetAsync()
    {
        if (BoardId.HasValue && BoardId.Value != _currentBoardId)
        {
            _currentBoardId = BoardId;
            Dispatcher.Dispatch(action: new GetBoardByIdAction(BoardId: BoardId.Value));

            try
            {
                GetBoardByIdResponse? getBoardByIdResponse = await BoardServiceApi.GetById(id: BoardId.Value);
                if (getBoardByIdResponse?.Board != null)
                {
                    Dispatcher.Dispatch(action: new GetBoardByIdSuccessAction(Board: getBoardByIdResponse.Board));
                    
                    Dispatcher.Dispatch(action: new GetAllColumnsAction(BoardId: _currentBoardId.Value));
                    GetAllColumnsResponse? getAllColumnsResponse = await ColumnServiceApi.GetAll(boardId: _currentBoardId.Value);
                    if (getAllColumnsResponse != null)
                    {
                        Dispatcher.Dispatch(action: new GetAllColumnsSuccessAction(Columns: getAllColumnsResponse.ListColumns));
                    }
                }
            }
            catch { /* ignored */ }
        }
    }

    private async Task OnColumnDrop()
    {
        if (!DragColumnState.IsDragging || !BoardId.HasValue)
        {
            return;
        }

        ColumnDto dragged = DragColumnState.DraggingColumn!;
        Guid? hoveredId = DragColumnState.HoveredColumnId;
        DragColumnState.Clear();
        
        if (hoveredId == null || hoveredId == dragged.Id)
        {
            return;
        }

        ColumnDto? target = ColumnListState.Value.Columns.FirstOrDefault(predicate: c => c.Id == hoveredId);
        if (target == null)
        {
            return;
        }
        
        try
        {
            await ColumnServiceApi.Update(id: dragged.Id, request: new UpdateColumnRequest
            {
                Name = dragged.Name,
                Color = dragged.Color,
                Order = target.Order
            });
            
            Dispatcher.Dispatch(action: new UpdateColumnSuccessAction(Column: dragged with { Order = target.Order }));
            Dispatcher.Dispatch(action: new UpdateColumnSuccessAction(Column: target with { Order = dragged.Order }));
        }
        catch
        {
            Dispatcher.Dispatch(action: new UpdateColumnSuccessAction(Column: dragged with { Order = dragged.Order }));
            Dispatcher.Dispatch(action: new UpdateColumnSuccessAction(Column: target with { Order = target.Order }));
        }
    }

    private void OpenEditBoard()
    {
        ModalUseState.Open(dialog: ModalUseState.ModalType.EditBoard);
    }
}
