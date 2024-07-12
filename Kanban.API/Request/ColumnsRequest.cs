using System.ComponentModel.DataAnnotations;

namespace Kanban.API.Request;
public record ColumnsRequest([Required] int BoardId, [Required] string Name, int Position);
