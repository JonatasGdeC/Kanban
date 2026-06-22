namespace Kanban.Communication.Requests.User;

public record ResetPasswordRequest
{
    public required string TokenResetPassword { get; set; }
    public required string NewPassword { get; set; }
}