
using Kanban.Adapter.Exceptions;
using Kanban.App.FluxorState.Board.Action;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Requests.Column;

namespace Kanban.App.Modals.Board.EditBoard;
using System.Threading.Tasks;


public partial class EditBoard
{
    private BoardDto Board => BoardState.Value.Board!;
    private List<ColumnDto> Columns => ModalUseState.Columns;

    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    private readonly RegisterBoardRequest _boardRequest = new() { Name = string.Empty };
    private readonly Dictionary<Guid, UpdateColumnRequest> _columnUpdateRequest = [];
    private readonly List<RegisterColumnRequest> _columnRegisterRequest = [];

    protected override void OnInitialized()
    {
        _boardRequest.Name = Board.Name;
        foreach (ColumnDto column in Columns)
        {
            _columnUpdateRequest[key: column.Id] = new UpdateColumnRequest
            {
                Name = column.Name,
                Color = column.Color,
                Order = column.Order
            };
        }
    }

    private async Task HandleRemoveColumnInBoard(Guid? columnId = null, RegisterColumnRequest? columnRequest = null)
    {
        if (columnId.HasValue)
        {
            await ColumnServiceApi.Delete(id: columnId.Value);
            ColumnUseState.Remove(boardId: Board.Id, columnId: columnId.Value);
            ModalUseState.Columns.Remove(item: Columns.First(predicate: c => c.Id == columnId.Value));
            _columnUpdateRequest.Remove(key: columnId.Value);
        }

        if (columnRequest != null)
        {
            _columnRegisterRequest.Remove(item: columnRequest);
        }
    }

    private void AddNewRegisterColumnRequest() => _columnRegisterRequest.Add(item: new RegisterColumnRequest
    {
        Name = string.Empty,
        Color = $"#{Random.Shared.Next(minValue: 100, maxValue: 999)}ff{Random.Shared.Next(minValue: 0, maxValue: 9)}"
    });

    private async Task HandleUpdateBoard()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            await BoardServiceApi.Update(id: Board.Id, request: _boardRequest);
            BoardDto updateBoard = new()
            {
                Id = Board.Id,
                Name = _boardRequest.Name
            };
            Dispatcher.Dispatch(action: new UpdateBoardSuccessAction(Board: updateBoard));

            int order = 0;
            foreach ((Guid id, UpdateColumnRequest req) in _columnUpdateRequest)
            {
                req.Order = order++;
                await ColumnServiceApi.Update(id: id, request: req);
                ColumnDto column = new()
                {
                    Id = id,
                    Name = req.Name,
                    Color = req.Color,
                    Order = req.Order
                };
                ColumnUseState.Set(boardId: Board.Id, column: column);
            }

            foreach (RegisterColumnRequest request in _columnRegisterRequest)
            {
                ColumnDto column = await ColumnServiceApi.Register(boardId: Board.Id, request: request);
                ColumnUseState.Set(boardId: Board.Id, column: column);
            }
            
            ModalUseState.Columns = ColumnUseState.List(boardId: Board.Id).ToList();
            ModalUseState.Close();
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "EDIT_BOARD_ERROR"]];
        }
        finally
        {
            _isSubmitting = false;
        }
    }
}
