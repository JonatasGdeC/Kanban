namespace Kanban.Communication.Requests.SubTask;

public record RegisterSubTaskRequest
{
    public required string Name { get; set; }
}
