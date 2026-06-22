
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class ColumnUseState : UseStateOnChange
{
    private Dictionary<(Guid boardId, Guid columnId), ColumnDto> _listColumns = [];

    public void Set(Guid boardId, ColumnDto column)
    {
        _listColumns[key: (boardId, column.Id)] = column;
        Notify();
    }

    public void Set(Guid boardId, List<ColumnDto> columns)
    {
        _listColumns = columns.ToDictionary(keySelector: column => (boardId, column.Id));
        Notify();
    }

    public void Clear()
    {
        _listColumns.Clear();
        Notify();
    }

    public void Remove(Guid boardId, Guid columnId)
    {
        _listColumns.Remove(key: (boardId, columnId));
        Notify(); 
    }

    public IReadOnlyList<ColumnDto> List(Guid boardId) => _listColumns.Where(predicate: kv => kv.Key.boardId == boardId).Select(selector: kv => kv.Value).ToList();
}
