namespace Kanban.Shared.Models.Models;

public class Tasks
{
  public Tasks()
  {
  }

  public Tasks(string title, string description, DateTime dueDate, int position)
  {
    Title = title;
    Description = description;
    DueDate = dueDate;
    Position = position;
    Subtasks = new List<Subtasks>();
  }

  public int Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public DateTime DueDate { get; set; }
  public int Position { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public int ColumnId { get; set; }
  public Columns Column { get; set; }
  public List<Subtasks> Subtasks { get; set; }
}
