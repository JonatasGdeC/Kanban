using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.SubTask.Action;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.SubTask;
using Kanban.Communication.Responses.Task;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kanban.App.Pages.Default.Components.TaskCard;

public partial class TaskCard : FluxorComponent
{
    [Parameter] public required TaskDto Task { get; set; }
    [Parameter] public required Guid ColumnId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        try
        {
            Dispatcher.Dispatch(action: new GetTaskByIdAction(TaskId: Task.Id));
            GetTaskByIdResponse? getTaskByIdResponse = await TaskServiceApi.GetById(id: Task.Id);
            
            if (getTaskByIdResponse != null)
            {
                Dispatcher.Dispatch(action: new GetTaskByIdSuccessAction(Task: getTaskByIdResponse.Task));
                
                Dispatcher.Dispatch(action: new GetAllSubTasksAction(TaskId: Task.Id));
                GetAllSubTasksResponse? getAllSubTasksResponse = await SubTaskServiceApi.GetAll(taskId: Task.Id);
                
                if (getAllSubTasksResponse != null)
                {
                    Dispatcher.Dispatch(action: new GetAllSubTasksSuccessAction(SubTasks: getAllSubTasksResponse.ListSubTasks));
                }
                
            }
        }
        catch { /* silently ignore load errors */ }
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
        ModalUseState.Subtasks = SubTaskListState.Value.SubTasks;
        ModalUseState.Open(dialog: ModalUseState.ModalType.ViewTask);
    }
}