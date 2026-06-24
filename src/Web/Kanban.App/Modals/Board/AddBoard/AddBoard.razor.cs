using Kanban.Adapter.Exceptions;
using Kanban.App.FluxorState.Board.Action;
using Kanban.App.FluxorState.Column.Action;
using Kanban.App.Services.SnackbarService;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Requests.Column;

namespace Kanban.App.Modals.Board.AddBoard;
using System.Threading.Tasks;

public partial class AddBoard
{
    private bool _isSubmitting;
    private readonly RegisterBoardRequest _boardRequest = new() { Name = string.Empty };
    private readonly List<RegisterColumnRequest> _columnRegisterRequest = [];
    private List<string> _listFeedbacks = [];

    private async Task HandleSubmitBoard()
    {
        _isSubmitting = true;

        try
        {
            BoardDto board = await BoardServiceApi.Register(board: _boardRequest);
            Dispatcher.Dispatch(action: new RegisterBoardSuccessAction(Board: board));

            foreach (RegisterColumnRequest request in _columnRegisterRequest)
            {
                ColumnDto column = await ColumnServiceApi.Register(boardId: board.Id, request: request);
                Dispatcher.Dispatch(action: new RegisterColumnSuccessAction(Column: column));
            }

            NavigationManager.NavigateTo(uri: $"/{board.Id}");
            SnackbarService.Show(message: ModalLocalizer[name: "MESSAGE_BOARD_CREATED_SUCCESS"], severity: SnackbarSeverity.Success);
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
