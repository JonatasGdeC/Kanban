
using Kanban.Communication.Dtos;

namespace Kanban.App.UseState;

public class ModalUseState : UseStateOnChange
{
    private ModalType? _current;
    public ModalType? Current => _current;
    public bool IsOpen => _current.HasValue;
    
    public void Open(ModalType dialog)
    {
        _current = dialog;
        Notify();
    }

    public void Close()
    {
        _current = null;
        Notify();
    }
    
    public BoardDto? Board { get; set; }
    public List<ColumnDto> Columns { get; set; } = [];
    public ColumnDto? Column { get; set; }
    public TaskDto? Task { get; set; }
    public List<SubTaskDto> Subtasks { get; set; } = [];

    public enum ModalType
    {
        None,
        ViewTask,
        AddTask,
        EditTask,
        DeleteTask,
        AddBoard,
        EditBoard,
        DeleteBoard,
        BoardOptions,
        EditProfile
    }
}