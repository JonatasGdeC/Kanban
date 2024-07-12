using System.ComponentModel.DataAnnotations;

namespace Kanban.WEB.Request;
public record ColumnsRequest([Required] int BoardId, [Required] string Name, int Position);
