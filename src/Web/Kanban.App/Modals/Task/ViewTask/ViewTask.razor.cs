using Kanban.Adapter.Exceptions;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.Task.Action;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Requests.Task;
using Kanban.Communication.Responses.SubTask;
using Microsoft.AspNetCore.Components;

namespace Kanban.App.Modals.Task.ViewTask;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ViewTask
{
    private bool _showTaskOptions;
    private List<SubTaskDto> _subtasks = [];
    private bool _subtasksLoaded;
    
    private TaskDto Task => ModalUseState.Task!;
    private List<ColumnDto> Columns => ColumnListState.Value.Columns;
    private List<string> _listFeedbacks = [];

    private Guid _columnId;

    protected override async Task OnInitializedAsync()
    {
        GetAllSubTasksResponse? response = await SubTaskServiceApi.GetAll(taskId: Task.Id);
        _subtasks = response?.ListSubTasks ?? [];
        _columnId = Task.ColumnId;
        _subtasksLoaded = true;
        StateHasChanged();
    }

    private async Task ToggleSubtask(SubTaskDto sub)
    {
        await SubTaskServiceApi.Update(id: sub.Id, request: new UpdateSubTaskRequest { Name = sub.Name, IsDone = !sub.IsDone });
        SubTaskDto subTaskUpdate = new()
        {
            Id = sub.Id,
            Name = sub.Name,
            IsDone = !sub.IsDone
        };
        
        Dispatcher.Dispatch(action: new UpdateSubTaskSuccessAction(SubTask: subTaskUpdate));
    }

    private async Task HandleUpdateTaskStatus(ChangeEventArgs e)
    {
        Guid newColumnId = Guid.Parse(input: e.Value!.ToString()!);

        try
        {
            int newOrder = TaskListState.Value.Tasks.Count;

            await TaskServiceApi.Update(id: Task.Id, request: new UpdateTaskRequest
            {
                Name = Task.Name,
                Description = Task.Description,
                ColumnId = newColumnId,
                Order = newOrder
            });

            TaskDto taskUpdate = new()
            {
                Id = Task.Id,
                Name = Task.Name,
                Description = Task.Description,
                Order = newOrder,
                ColumnId = newColumnId
            };

            Dispatcher.Dispatch(action: new UpdateTaskSuccessAction(Task: taskUpdate));
            _columnId = newColumnId;
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
}
