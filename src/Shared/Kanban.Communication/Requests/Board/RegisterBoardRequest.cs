namespace Kanban.Communication.Requests.Board;

public record RegisterBoardRequest
{
    public required string Name { get; set; }
}