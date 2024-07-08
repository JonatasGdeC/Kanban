namespace Kanban.Shared.Models.Models;

public class Subtasks
{
  public Subtasks()
  {
  }

  public Subtasks(string title, bool isCompleted)
  {
    Title = title;
    IsCompleted = isCompleted;
  }

  public int Id { get; set; }
  public string Title { get; set; }
  public bool IsCompleted { get; set; }

  public DateTime CreatedAt { get; set; }
  public int TaskId { get; set; }
  public Tasks Task { get; set; }
}
