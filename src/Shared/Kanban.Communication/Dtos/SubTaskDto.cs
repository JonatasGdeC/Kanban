namespace Kanban.Communication.Dtos;

public record SubTaskDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public bool IsDone { get; init; }
}