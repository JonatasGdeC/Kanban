namespace Kanban.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public int Order { get; set; }
    public List<TaskEntity> Tasks { get; set; } = [];
    
    public required Guid BoardId { get; set; }
    public required Board Board { get; set; }
}