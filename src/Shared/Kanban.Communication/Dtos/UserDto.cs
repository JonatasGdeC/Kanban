namespace Kanban.Communication.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}