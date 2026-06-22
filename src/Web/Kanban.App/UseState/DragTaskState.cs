
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class DragTaskState
{
    public TaskDto? DraggingTask { get; private set; }
    public Guid SourceColumnId { get; private set; }
    public bool IsDragging => DraggingTask != null;

    public void Start(TaskDto task, Guid sourceColumnId)
    {
        DraggingTask = task;
        SourceColumnId = sourceColumnId;
    }

    public void Clear()
    {
        DraggingTask = null;
        SourceColumnId = Guid.Empty;
    }
}
