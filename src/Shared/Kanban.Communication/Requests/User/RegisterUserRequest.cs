namespace Kanban.Communication.Requests.User;

public record RegisterUserRequest : UpdateUserRequest
{
    public required string Password { get; set; }
}
