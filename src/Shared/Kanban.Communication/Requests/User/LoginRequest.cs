namespace Kanban.Communication.Requests.User;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
