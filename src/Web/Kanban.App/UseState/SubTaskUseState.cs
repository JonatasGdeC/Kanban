
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class SubTaskUseState : UseStateOnChange
{
    private Dictionary<(Guid taskId, Guid subTaskId), SubTaskDto> _listSubTasks = [];

    public void Set(Guid taskId, SubTaskDto subTask)
    {
        _listSubTasks[key: (taskId, subTask.Id)] = subTask;
        Notify();
    }

    public void Set(Guid taskId, List<SubTaskDto> subTasks)
    {
        _listSubTasks = subTasks.ToDictionary(keySelector: subTask => (taskId, subTask.Id));
        Notify();
    }

    public void Clear()
    {
        _listSubTasks.Clear();
        Notify();
    }

    public void Remove(Guid taskId, Guid subTaskId)
    {
        _listSubTasks.Remove(key: (taskId, subTaskId));
        Notify();
    }
    
    public IReadOnlyList<SubTaskDto> List(Guid taskId) => _listSubTasks.Where(predicate: kv => kv.Key.taskId == taskId).Select(selector: kv => kv.Value).ToList();
}
