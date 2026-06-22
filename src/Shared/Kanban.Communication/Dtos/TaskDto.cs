namespace Kanban.Communication.Dtos;

public record TaskDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public Guid ColumnId { get; init; }
    public int Order { get; init; }
}