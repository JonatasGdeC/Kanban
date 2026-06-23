using Kanban.App.FluxorState.Action.Board;
using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Column;
using Kanban.Communication.Responses.Board;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Pages.Default.Main;

public partial class Default : IDisposable
{
    [Parameter] public Guid? BoardId { get; set; }

    private BoardDto? _currentBoard;
    private List<ColumnDto> CurrentColumns => BoardId.HasValue ? ColumnUseState.List(boardId: BoardId.Value).ToList() : [];

    private Guid? _currentBoardId;
    private bool _isBoardLoading = true;

    // protected override void OnInitialized()
    // {
    //     BoardUseState.OnChange += StateHasChanged;
    //     ColumnUseState.OnChange += StateHasChanged;
    //     ModalUseState.Board = _currentBoard;
    //     ModalUseState.Columns = CurrentColumns;
    // }

    protected override async Task OnParametersSetAsync()
    {
        _isBoardLoading = true;
      
        try
        {
            if (BoardId.HasValue && BoardId.Value != _currentBoardId)
            {
                ClearUseStates();
                
                Dispatcher.Dispatch(action: new LoadBoardAction(BoardId: BoardId.Value));
                _currentBoardId = BoardId;

                // GetBoardByIdResponse? response = await BoardServiceApi.GetById(id: BoardId.Value);
                // if (response != null)
                // {
                //     _currentBoard = response.Board;
                //     BoardUseState.Set(board: response.Board);
                //     ColumnUseState.Set(boardId: _currentBoard.Id, columns: response.Columns);
                //
                //     ModalUseState.Board = response.Board;
                //     ModalUseState.Columns = response.Columns;
                // }
            }
        }
        catch { /* silently ignore load errors */ }
        finally
        {
            _isBoardLoading = false;
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

        ColumnDto? target = CurrentColumns.FirstOrDefault(predicate: c => c.Id == hoveredId);
        if (target == null)
        {
            return;
        }

        // Swap otimista no state
        ColumnUseState.Set(boardId: BoardId.Value, column: dragged with { Order = target.Order });
        ColumnUseState.Set(boardId: BoardId.Value, column: target with { Order = dragged.Order });

        try
        {
            await ColumnServiceApi.Update(id: dragged.Id, request: new UpdateColumnRequest
            {
                Name = dragged.Name,
                Color = dragged.Color,
                Order = target.Order
            });
        }
        catch
        {
            // Reverte o swap no state em caso de erro
            ColumnUseState.Set(boardId: BoardId.Value, column: dragged with { Order = dragged.Order });
            ColumnUseState.Set(boardId: BoardId.Value, column: target with { Order = target.Order });
        }
    }

    private void OpenEditBoard()
    {
        ModalUseState.Board = _currentBoard;
        ModalUseState.Columns = CurrentColumns;
        ModalUseState.Open(dialog: ModalUseState.ModalType.EditBoard);
    }

    private void ClearUseStates()
    {
        ColumnUseState.Clear();
        TaskUseState.Clear();
        SubTaskUseState.Clear();
    }

    public void Dispose()
    {
        BoardUseState.OnChange -= StateHasChanged;
        ColumnUseState.OnChange -= StateHasChanged;
    }
}
