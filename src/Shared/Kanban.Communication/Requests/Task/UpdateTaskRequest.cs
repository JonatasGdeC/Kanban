namespace Kanban.Communication.Requests.Task;

public record UpdateTaskRequest : RegisterTaskRequest
{
    public int Order { get; set; }
    public Guid ColumnId { get; set; }
}
