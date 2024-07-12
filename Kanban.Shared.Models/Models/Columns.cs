namespace Kanban.Shared.Models.Models;

public class Columns
{
  public Columns()
  {
  }

  public Columns(string name, int position)
  {
    Name = name;
    Position = position;
    Tasks = new List<Tasks>();
  }

  public int Id { get; set; }
  public string? Name { get; set; }
  public int Position { get; set; }

  public int BoardId { get; set; }
  public Boards Board { get; set; }

  public List<Tasks> Tasks { get; set; }
}
