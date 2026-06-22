namespace Kanban.Communication.Requests.User;

public record ForgotPasswordRequest
{
    public required string Email { get; set; }
}