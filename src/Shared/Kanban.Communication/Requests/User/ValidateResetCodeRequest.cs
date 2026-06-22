namespace Kanban.Communication.Requests.User;

public record ValidateResetCodeRequest
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}