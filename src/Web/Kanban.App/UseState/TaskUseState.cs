
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class TaskUseState : UseStateOnChange
{
    private Dictionary<(Guid columnId, Guid taskId), TaskDto> _listTasks = [];

    public void Set(Guid columnId, TaskDto task)
    {
        _listTasks[key: (columnId, task.Id)] = task;
        Notify();
    }

    public void Set(Guid columnId, List<TaskDto> tasks)
    {
        foreach ((Guid columnId, Guid taskId) key in _listTasks.Keys.Where(predicate: k => k.columnId == columnId).ToList())
            _listTasks.Remove(key: key);

        foreach (TaskDto task in tasks)
            _listTasks[key: (columnId, task.Id)] = task;

        Notify();
    }

    public void Clear()
    {
        _listTasks.Clear();
        Notify();
    }

    public void Remove(Guid columnId, Guid itemId)
    {
        _listTasks.Remove(key: (columnId, itemId));
        Notify();
    }
    
    public IReadOnlyList<TaskDto> List(Guid columnId) => _listTasks.Where(predicate: kv => kv.Key.columnId == columnId).Select(selector: kv => kv.Value).ToList();
}
