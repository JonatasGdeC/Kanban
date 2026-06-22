using Kanban.Communication.Dtos;

namespace Kanban.Communication.Responses.User;

public record RegisteredUserResponse
{
    public required UserDto User { get; init; }
    public required string Token { get; init; }
}
