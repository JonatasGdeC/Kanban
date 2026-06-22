namespace Kanban.Communication.Requests.SubTask;

public record UpdateSubTaskRequest : RegisterSubTaskRequest
{
    public bool IsDone { get; set; }
}
