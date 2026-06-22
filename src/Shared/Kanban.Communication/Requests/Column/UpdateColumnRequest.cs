namespace Kanban.Communication.Requests.Column;

public record UpdateColumnRequest : RegisterColumnRequest
{
    public int Order { get; set; }
}