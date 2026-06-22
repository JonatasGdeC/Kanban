namespace Kanban.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<Column> Columns { get; set; } = [];
    
    public required Guid UserId { get; set; }
}