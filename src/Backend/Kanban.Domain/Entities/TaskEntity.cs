namespace Kanban.Domain.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public List<SubTask> SubTasks { get; set; } = [];
    public required Guid ColumnId { get; set; }
    public required Column Column { get; set; }
}