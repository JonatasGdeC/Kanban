using System.ComponentModel.DataAnnotations;

namespace Kanban.WEB.Request;

public record TasksRequest([Required] int ColumnId, [Required] string Title, string Description, int Position);
