namespace Kanban.Communication.Requests.User;

public record UpdateUserRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}
