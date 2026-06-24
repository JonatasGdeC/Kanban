using Kanban.App.FluxorState.Task.Action;
using Kanban.Communication.Dtos;

namespace Kanban.App.Modals.Task.DeleteTask;
using System.Threading.Tasks;

public partial class DeleteTask
{
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    
    private TaskDto Task => ModalUseState.Task!;

    private async Task HandleDelete()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            await TaskServiceApi.Delete(id: Task.Id);
            Dispatcher.Dispatch(action: new DeleteTaskSuccessAction(TaskId: Task.Id));
            ModalUseState.Close();
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "DELETE_TASK_ERROR"]];
        }
        finally
        {
            _isSubmitting = false;
        }
    }
}
