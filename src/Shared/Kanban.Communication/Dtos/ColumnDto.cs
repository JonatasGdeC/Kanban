namespace Kanban.Communication.Dtos;

public record ColumnDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Color { get; init; }
    public int Order { get; init; }
}