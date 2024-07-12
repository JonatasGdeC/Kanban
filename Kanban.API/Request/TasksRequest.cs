using System.ComponentModel.DataAnnotations;

namespace Kanban.API.Request;

public record TasksRequest([Required] int ColumnId, [Required] string Title, string Description, int Position);
