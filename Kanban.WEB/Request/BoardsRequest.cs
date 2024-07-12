using System.ComponentModel.DataAnnotations;

namespace Kanban.WEB.Request;
public record BoardsRequest([Required] string Name, DateTime CreatedAt);
