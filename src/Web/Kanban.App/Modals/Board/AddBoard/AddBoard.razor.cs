using Kanban.Adapter.Exceptions;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Requests.Column;

namespace Kanban.App.Modals.Board.AddBoard;
using System.Threading.Tasks;

public partial class AddBoard
{
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    private readonly RegisterBoardRequest _boardRequest = new() { Name = string.Empty };
    private readonly List<RegisterColumnRequest> _columnRegisterRequest = [];

    private async Task HandleSubmitBoard()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            BoardDto board = await BoardServiceApi.Register(board: _boardRequest);

            foreach (RegisterColumnRequest request in _columnRegisterRequest)
            {
                ColumnDto column = await ColumnServiceApi.Register(boardId: board.Id, request: request);
                ColumnUseState.Set(boardId: board.Id, column: column);
            }

            BoardUseState.Set(board: board);
            NavigationManager.NavigateTo(uri: $"/{board.Id}");
            ModalUseState.Board = board;
            ModalUseState.Columns = ColumnUseState.List(boardId: board.Id).ToList();
            ModalUseState.Close();
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "ADD_BOARD_ERROR"]];
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    
    
    private void AddNewRegisterColumnRequest() => _columnRegisterRequest.Add(item: new RegisterColumnRequest
    {
        Name = string.Empty,
        Color = $"#{Random.Shared.Next(minValue: 100, maxValue: 999)}ff{Random.Shared.Next(minValue: 0, maxValue: 9)}"
    });

    private void AddRemoveRegisterColumnRequest(RegisterColumnRequest column) => _columnRegisterRequest.Remove(item: column);
}
