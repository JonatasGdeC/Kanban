using Kanban.App.FluxorState.Board.Action;
using Kanban.App.Services.SnackbarService;
using Kanban.Communication.Dtos;

namespace Kanban.App.Modals.Board.DeleteBoard;
using System.Threading.Tasks;


public partial class DeleteBoard
{
    private BoardDto Board => BoardState.Value.Board!;
    
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];

    private async Task HandleDeleteBoard()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            Guid boardId = Board.Id;
            await BoardServiceApi.Delete(id: boardId);
            Dispatcher.Dispatch(action: new DeleteBoardSuccessAction(BoardId: boardId));
            ModalUseState.Board = null;
            NavigationManager.NavigateTo(uri: "/");
            SnackbarService.Show(message: ModalLocalizer[name: "MESSAGE_BOARD_DELETED_SUCCESS"], severity: SnackbarSeverity.Info);
            ModalUseState.Close();
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "DELETE_BOARD_ERROR"]];
        }
        finally
        {
            _isSubmitting = false;
        }
    }
}
