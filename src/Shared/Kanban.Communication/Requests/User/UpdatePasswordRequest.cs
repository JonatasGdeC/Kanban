namespace Kanban.Communication.Requests.User;

public record UpdatePasswordRequest
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}