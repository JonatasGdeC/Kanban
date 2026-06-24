using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.Services.SnackbarService;
using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.SubTask;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kanban.App.Pages.Default.Components.TaskCard;

public partial class TaskCard : FluxorComponent
{
    [Parameter] public required TaskDto Task { get; set; }
    [Parameter] public required Guid ColumnId { get; set; }

    private List<SubTaskDto> TaskSubTasks =>
        SubTaskListState.Value.SubTasksByTaskId.GetValueOrDefault(key: Task.Id, defaultValue: []);

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            Dispatcher.Dispatch(action: new GetAllSubTasksAction(TaskId: Task.Id));
            GetAllSubTasksResponse? getAllSubTasksResponse = await SubTaskServiceApi.GetAll(taskId: Task.Id);

            if (getAllSubTasksResponse != null)
            {
                Dispatcher.Dispatch(action: new GetAllSubTasksSuccessAction(TaskId: Task.Id,
                    SubTasks: getAllSubTasksResponse.ListSubTasks));
            }
        }
        catch
        {
            SnackbarService.Show(message: DefaultLocalizer[name: "ERROR_LOADING_SUBTASKS"], severity: SnackbarSeverity.Error);
        }
    }

    private void OnDragStart(DragEventArgs e)
    {
        DragTaskState.Start(task: Task, sourceColumnId: ColumnId);
    }

    private void OnDragEnd(DragEventArgs e)
    {
        DragTaskState.Clear();
    }

    private void OpenViewTask()
    {
        if (DragTaskState.IsDragging)
        {
            return;
        }
        
        ModalUseState.Task = Task;
        ModalUseState.Subtasks = TaskSubTasks;
        ModalUseState.Open(dialog: ModalUseState.ModalType.ViewTask);
    }
}