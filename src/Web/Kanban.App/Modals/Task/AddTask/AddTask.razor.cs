using Kanban.Adapter.Exceptions;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Requests.Task;

namespace Kanban.App.Modals.Task.AddTask;
using System.Threading.Tasks;

public partial class AddTask
{
    private List<ColumnDto> Columns => ColumnListState.Value.Columns;

    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    private readonly RegisterTaskRequest _registerTaskRequest = new()
    {
        Name = string.Empty,
        Description = null,
    };
    
    private Guid _selectedColumnId = Guid.Empty;
    private readonly List<RegisterSubTaskRequest> _subTaskRegisterRequests = [];

    private async Task HandleSubmit()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            TaskDto task = await TaskServiceApi.Register(columnId: _selectedColumnId, request: _registerTaskRequest);
            Dispatcher.Dispatch(action: new RegisterTaskSuccessAction(Task: task));
            
            foreach (RegisterSubTaskRequest subTaskRequest in _subTaskRegisterRequests)
            {
                SubTaskDto subTask = await SubTaskServiceApi.Register(taskId: task.Id, request: subTaskRequest);
                Dispatcher.Dispatch(action: new RegisterSubTaskSuccessAction(TaskId: task.Id, SubTask: subTask));
            }

            ModalUseState.Close();
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "ADD_TASK_ERROR"]];
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void HandleAddNewSubTaskRequest() => _subTaskRegisterRequests.Add(item: new RegisterSubTaskRequest {Name = string.Empty});
    private void HandleRemoveSubTaskRequest(RegisterSubTaskRequest request) => _subTaskRegisterRequests.Remove(item: request);
}
