using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.Task.Action;
using Kanban.App.Services.SnackbarService;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Task;
using Kanban.Communication.Responses.Task;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kanban.App.Pages.Default.Components.ColumnContainer;

public partial class ColumnContainer : FluxorComponent
{
    [Parameter] public required ColumnDto Column { get; set; }

    private List<TaskDto> ColumnTasks => TaskListState.Value.Tasks
        .Where(predicate: t => t.ColumnId == Column.Id)
        .OrderBy(keySelector: t => t.Order)
        .ToList();

    private int _dropIndex = -1;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            Dispatcher.Dispatch(action: new GetAllTasksAction(ColumnId: Column.Id));
            GetAllTasksResponse? response = await TaskServiceApi.GetAll(columnId: Column.Id);

            if (response != null)
            {
                Dispatcher.Dispatch(action: new GetAllTasksSuccessAction(Tasks: response.ListTasks));
            }
        }
        catch
        {
            SnackbarService.Show(message: DefaultLocalizer[name: "ERROR_LOADING_TASKS"], severity: SnackbarSeverity.Error);
        }

        _isLoading = false;
    }

    private void OnColumnDragStart(DragEventArgs e)
    {
        DragColumnState.Start(column: Column);
    }

    private void OnColumnDragEnd(DragEventArgs e)
    {
        DragColumnState.Clear();
    }

    private void OnDragOverTask(int index)
    {
        if (!DragTaskState.IsDragging)
        {
            return;
        }

        _dropIndex = index;
    }

    private void OnDragOverColumn()
    {
        if (DragColumnState.IsDragging)
        {
            DragColumnState.SetHovered(columnId: Column.Id);
            return;
        }

        if (!DragTaskState.IsDragging)
        {
            return;
        }

        if (_dropIndex == -1)
        {
            _dropIndex = 0;
        }
    }

    private async Task HandleDrop()
    {
        if (DragColumnState.IsDragging)
        {
            DragColumnState.SetHovered(columnId: Column.Id);
            return;
        }

        if (!DragTaskState.IsDragging)
        {
            return;
        }

        try
        {
            TaskDto dragged = DragTaskState.DraggingTask!;
            Guid targetColumnId = Column.Id;
            
            List<TaskDto> targetTasks = TaskListState.Value.Tasks
                .Where(predicate: t => t.ColumnId == targetColumnId && t.Id != dragged.Id)
                .OrderBy(keySelector: t => t.Order)
                .ToList();

            int insertAt = Math.Clamp(value: _dropIndex == -1 ? targetTasks.Count : _dropIndex, min: 0,
                max: targetTasks.Count);
            
            await TaskServiceApi.Update(id: dragged.Id, request: new UpdateTaskRequest
            {
                Name = dragged.Name,
                Description = dragged.Description,
                ColumnId = targetColumnId,
                Order = insertAt
            });
            
            targetTasks.Insert(index: insertAt, item: dragged);

            for (int i = 0; i < targetTasks.Count; i++)
            {
                TaskDto updated = targetTasks[index: i] with { Order = i, ColumnId = targetColumnId };
                Dispatcher.Dispatch(action: new UpdateTaskSuccessAction(Task: updated));
            }

            DragTaskState.Clear();
            _dropIndex = -1;
        }
        catch
        {
            SnackbarService.Show(message: DefaultLocalizer[name: "ERROR_UPDATING_TASK"], severity: SnackbarSeverity.Error);
        }
    }
}