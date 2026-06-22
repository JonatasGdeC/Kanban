
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class DragColumnState
{
    public ColumnDto? DraggingColumn { get; private set; }
    public Guid? HoveredColumnId { get; private set; }
    public bool IsDragging => DraggingColumn != null;

    public void Start(ColumnDto column)
    {
        DraggingColumn = column;
        HoveredColumnId = null;
    }

    public void SetHovered(Guid columnId)
    {
        HoveredColumnId = columnId;
    }

    public void Clear()
    {
        DraggingColumn = null;
        HoveredColumnId = null;
    }
}
