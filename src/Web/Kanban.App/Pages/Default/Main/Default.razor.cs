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
        // if (!DragColumnState.IsDragging || !BoardId.HasValue)
        // {
        //     return;
        // }
        //
        // ColumnDto dragged = DragColumnState.DraggingColumn!;
        // Guid? hoveredId = DragColumnState.HoveredColumnId;
        //
        // DragColumnState.Clear();
        //
        // if (hoveredId == null || hoveredId == dragged.Id)
        // {
        //     return;
        // }
        //
        // ColumnDto? target = CurrentColumns.FirstOrDefault(predicate: c => c.Id == hoveredId);
        // if (target == null)
        // {
        //     return;
        // }
        //
        // // Swap otimista no state
        // ColumnUseState.Set(boardId: BoardId.Value, column: dragged with { Order = target.Order });
        // ColumnUseState.Set(boardId: BoardId.Value, column: target with { Order = dragged.Order });
        //
        // try
        // {
        //     await ColumnServiceApi.Update(id: dragged.Id, request: new UpdateColumnRequest
        //     {
        //         Name = dragged.Name,
        //         Color = dragged.Color,
        //         Order = target.Order
        //     });
        // }
        // catch
        // {
        //     // Reverte o swap no state em caso de erro
        //     ColumnUseState.Set(boardId: BoardId.Value, column: dragged with { Order = dragged.Order });
        //     ColumnUseState.Set(boardId: BoardId.Value, column: target with { Order = target.Order });
        // }
    }

    private void OpenEditBoard()
    {
        ModalUseState.Open(dialog: ModalUseState.ModalType.EditBoard);
    }
}
