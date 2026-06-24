using Fluxor.Blazor.Web.Components;
using Kanban.App.FluxorState.Task.Action;
using Kanban.Communication.Dtos;
using Kanban.Communication.Responses.Task;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Kanban.App.Pages.Default.Components.ColumnContainer;

public partial class ColumnContainer : FluxorComponent
{
    [Parameter] public required ColumnDto Column { get; set; }

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
        catch { /* silently ignore load errors */ }
        
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
        // if (DragColumnState.IsDragging)
        // {
        //     // Delega o swap para o Default via borbulhamento — apenas limpa o hover
        //     DragColumnState.SetHovered(columnId: Column.Id);
        //     return;
        // }
        //
        // if (!DragTaskState.IsDragging)
        // {
        //     return;
        // }
        //
        // TaskDto dragged = DragTaskState.DraggingTask!;
        // Guid sourceColumnId = DragTaskState.SourceColumnId;
        // Guid targetColumnId = Column.Id;
        //
        // List<TaskDto> targetTasks = TaskUseState
        //     .List(columnId: targetColumnId)
        //     .OrderBy(keySelector: t => t.Order)
        //     .Where(predicate: t => t.Id != dragged.Id)
        //     .ToList();
        //
        // int insertAt = Math.Clamp(value: _dropIndex == -1 ? targetTasks.Count : _dropIndex, min: 0, max: targetTasks.Count);
        //
        // targetTasks.Insert(index: insertAt, item: dragged);
        //
        // if (sourceColumnId != targetColumnId)
        // {
        //     TaskUseState.Remove(columnId: sourceColumnId, itemId: dragged.Id);
        // }
        //
        // for (int i = 0; i < targetTasks.Count; i++)
        // {
        //     TaskDto updated = targetTasks[index: i] with { Order = i, ColumnId = targetColumnId };
        //     TaskUseState.Set(columnId: targetColumnId, task: updated);
        // }
        //
        // DragTaskState.Clear();
        // _dropIndex = -1;
        //
        // try
        // {
        //     await TaskServiceApi.Update(id: dragged.Id, request: new UpdateTaskRequest
        //     {
        //         Name = dragged.Name,
        //         Description = dragged.Description,
        //         ColumnId = targetColumnId,
        //         Order = insertAt
        //     });
        // }
        // catch { /* silently ignore — state already reflects local change */ }
    }
}
