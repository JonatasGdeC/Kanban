namespace Kanban.Communication.Dtos;

public record BoardDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}