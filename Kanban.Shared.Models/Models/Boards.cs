namespace Kanban.Shared.Models.Models;

public class Boards
{
    public Boards () {}

    public Boards(string name, DateTime createdAt)
    {
        Name = name;
        CreatedAt = createdAt;
        Columns = new List<Columns>();
    }
    
    public int Id { get; set; } 
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Columns> Columns { get; set; }
}