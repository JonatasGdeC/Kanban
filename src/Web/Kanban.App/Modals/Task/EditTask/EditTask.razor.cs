using Kanban.Adapter.Exceptions;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Requests.Task;

namespace Kanban.App.Modals.Task.EditTask;
using System.Threading.Tasks;

public partial class EditTask
{
    private TaskDto Task => ModalUseState.Task!;
    private List<SubTaskDto> Subtasks => ModalUseState.Subtasks;
    private List<ColumnDto> Columns => ModalUseState.Columns;
    
    private bool _isSubmitting;
    private List<string> _listFeedbacks = [];
    private readonly UpdateTaskRequest _taskRequest = new() { Name = string.Empty };
    private readonly Dictionary<Guid, UpdateSubTaskRequest> _subtaskUpdateRequest = new();
    private readonly List<RegisterSubTaskRequest> _subTaskRegisterRequests = [];

    protected override void OnInitialized()
    {
        _taskRequest.Name = Task.Name;
        _taskRequest.Description = Task.Description;
        _taskRequest.Order = Task.Order;
        _taskRequest.ColumnId = Task.ColumnId;

        foreach (SubTaskDto sub in Subtasks)
        {
            _subtaskUpdateRequest[key: sub.Id] = new UpdateSubTaskRequest { Name = sub.Name, IsDone = sub.IsDone };
        }
        
        StateHasChanged();
    }

    private async Task SubmitUpdateTask()
    {
        _isSubmitting = true;
        _listFeedbacks = [];

        try
        {
            await HandleUpdateTask();

            if (_subTaskRegisterRequests.Any())
            {
                foreach (RegisterSubTaskRequest subTaskRequest in _subTaskRegisterRequests)
                {
                    await HandleRegisterSubTask(request: subTaskRequest);
                }
            }

            ModalUseState.Close();
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private async Task HandleRemoveSubTask(Guid subTaskId)
    {
        await SubTaskServiceApi.Delete(id: subTaskId);
        SubTaskUseState.Remove(taskId: Task.Id, subTaskId: subTaskId);
        _subtaskUpdateRequest.Remove(key: subTaskId);
    }

    private async Task HandleUpdateTask()
    {
        try
        {
            bool isChangingColumn = _taskRequest.ColumnId != Task.ColumnId;

            if (isChangingColumn)
            {
                _taskRequest.Order = TaskUseState.List(columnId: _taskRequest.ColumnId).Count;
            }

            await TaskServiceApi.Update(id: Task.Id, request: _taskRequest);

            TaskDto taskUpdate = new()
            {
                Id = Task.Id,
                Name = _taskRequest.Name,
                Description = _taskRequest.Description,
                ColumnId = _taskRequest.ColumnId,
                Order = _taskRequest.Order
            };

            TaskUseState.Remove(columnId: Task.ColumnId, itemId: taskUpdate.Id);
            TaskUseState.Set(columnId: taskUpdate.ColumnId, task: taskUpdate);
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
            throw;
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "EDIT_TASK_ERROR"]];
            throw;
        }
    }

    private async Task HandleRegisterSubTask(RegisterSubTaskRequest request)
    {
        try
        {
            SubTaskDto response = await SubTaskServiceApi.Register(taskId: Task.Id, request: request);
            SubTaskUseState.Set(taskId: Task.Id, subTask: response);
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
            throw;
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "ADD_SUBTASK_ERROR"]];
            throw;
        }
    }

    private void HandleRemoveRegisterSubTask(RegisterSubTaskRequest request) => _subTaskRegisterRequests.Remove(item: request);
    private void HandleAddRegisterSubTask() => _subTaskRegisterRequests.Add(item: new RegisterSubTaskRequest { Name = string.Empty });

    private async Task SubmitUpdateSubTask(Guid subTaskId, UpdateSubTaskRequest request)
    {
        try
        {
            await SubTaskServiceApi.Update(id: subTaskId, request: request);
            SubTaskDto subTaskUpdate = new()
            {
                Id = subTaskId,
                Name = request.Name,
                IsDone = request.IsDone
            };
            SubTaskUseState.Set(taskId: Task.Id, subTask: subTaskUpdate);
        }
        catch (ApiException exception) when (exception.ErrorMessages.Count > 0)
        {
            _listFeedbacks = exception.ErrorMessages.ToList();
            throw;
        }
        catch
        {
            _listFeedbacks = [ModalLocalizer[name: "UPDATE_SUBTASK_ERROR"]];
            throw;
        }
    }
}
