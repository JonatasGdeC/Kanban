namespace Kanban.Communication.Requests.Column;

public record RegisterColumnRequest
{
    public required string Name { get; set; }
    public required string Color { get; set; }
}
