namespace Kanban.Communication.Responses.User;

public record ValidateResetCodeResponse
{
    public required string TokenResetPassword { get; init; }
}