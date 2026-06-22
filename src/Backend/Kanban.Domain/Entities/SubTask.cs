namespace Kanban.Domain.Entities;

public class SubTask
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsDone { get; set; }
    public required Guid TaskId { get; set; }
    public required TaskEntity Task { get; set; }
}