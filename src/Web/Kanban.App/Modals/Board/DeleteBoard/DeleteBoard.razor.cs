
using Kanban.Communication.Dtos;

namespace Kanban.App.Modals.Board.DeleteBoard;
using System.Threading.Tasks;


public partial class DeleteBoard
{
    private BoardDto Board => ModalUseState.Board!;

    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];

    private async Task HandleDeleteBoard()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            Guid currentBoardId = Board.Id;
            
            await BoardServiceApi.Delete(id: currentBoardId);
            ModalUseState.Board = null;
            BoardUseState.Remove(itemId: currentBoardId);
            NavigationManager.NavigateTo(uri: "/");
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
