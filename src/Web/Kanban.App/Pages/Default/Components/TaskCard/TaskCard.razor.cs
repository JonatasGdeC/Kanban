using Kanban.App.UseState;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Task;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kanban.App.Pages.Default.Components.TaskCard;

public partial class TaskCard : IDisposable
{
    [Parameter] public required TaskDto Task { get; set; }
    [Parameter] public required Guid ColumnId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        SubTaskUseState.OnChange += StateHasChanged;

        try
        {
            GetTaskByIdResponse? response = await TaskServiceApi.GetById(id: Task.Id);
            if (response != null)
            {
                Task = response.Task;
                SubTaskUseState.Set(taskId: Task.Id, subTasks: response.SubTasks);
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
        ModalUseState.Subtasks = SubTaskUseState.List(taskId: Task.Id).ToList();
        ModalUseState.Open(dialog: ModalUseState.ModalType.ViewTask);
    }

    public void Dispose()
    {
        SubTaskUseState.OnChange -= StateHasChanged;
    }
}